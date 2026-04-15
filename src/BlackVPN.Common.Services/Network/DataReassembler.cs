using BlackProjects.Common.Models.Packets;
using System.Collections.Concurrent;

namespace BlackProjects.Common.Services.Network;
public class DataReassembler : IDataReassembler {

    private readonly ConcurrentDictionary
        <(Guid SessionId, uint MessageId), FragmentBuffer> buffers = new();
    private readonly TimeSpan ttl = TimeSpan.FromSeconds(30);
    public ReassembledMessage? Push (
            Guid sessionId,
            VpnFragmentHeader header,
            ReadOnlyMemory<byte> ciphertext,
            ReadOnlyMemory<byte>? nonce,
            ReadOnlyMemory<byte>? tag) {
        CleanupExpired();
        if (header.FragmentCount == 0) { return null; }
        if (header.FragmentIndex >= header.FragmentCount){
            return null;
        }
        var key = (sessionId, header.MessageId);
        var buffer = buffers.GetOrAdd(key, _ =>
            new FragmentBuffer(
                header.FragmentCount,
                header.Flags.HasFlag(VpnFragmentFlags.First)
                    ? nonce?.ToArray()
                    : null,
                header.Flags.HasFlag(VpnFragmentFlags.First)
                    ? tag?.ToArray()
                    : null
            )
        );
        // Если это первый фрагмент (с флагом First), сохраняем nonce/tag
        if (header.Flags.HasFlag(VpnFragmentFlags.First)) {
            buffer.SetCryptoMetadata(nonce, tag);
        }
        buffer.Add(header.FragmentIndex, ciphertext);
        if (!buffer.IsComplete) { return null; }  
        // Фрагмент с nonce/tag не пришел - ждем
        if (!buffer.HasCryptoMetadata) { return null; }
        buffers.TryRemove(key, out _);
        return new ReassembledMessage {
            Ciphertext = buffer.Assemble(),
            Nonce = buffer.Nonce!,
            Tag = buffer.Tag!
        };
    }

    public void Reset (Guid sessionId) {
        foreach (var key in buffers.Keys) {
            if (key.SessionId == sessionId) {
                buffers.TryRemove(key, out _);
            }
        }
    }

    private void CleanupExpired () {
        var now = DateTime.UtcNow;
        foreach (var kv in buffers) {
            if (now - kv.Value.Created > ttl) {
                buffers.TryRemove(kv.Key, out _);
            }
        }
    }

    private sealed class FragmentBuffer {
        private readonly ReadOnlyMemory<byte>?[] parts;
        private int received;
        private readonly object lockObj = new();
        public DateTime Created { get; } = DateTime.UtcNow;
        private byte[]? nonce;
        private byte[]? tag;
        public byte[]? Nonce => nonce;
        public byte[]? Tag => tag;
        public bool HasCryptoMetadata => nonce != null && tag != null;
        public FragmentBuffer (
                int count,
                byte[]? nonce,
                byte[]? tag) {
            parts = new ReadOnlyMemory<byte>?[count];
            this.nonce = nonce;
            this.tag = tag;
        }

        public bool IsComplete
        {
            get {
                lock (lockObj) {
                    return received == parts.Length;
                }
            }
        }

        public void Add (int index, ReadOnlyMemory<byte> payload) {
            if (index < 0 || index >= parts.Length) { return; }
            if (parts[index] != null) { return; }
            parts[index] = payload;
            received++;
        }
        /// <summary>
        /// Устанавливает nonce и tag из первого фрагмента (может прийти в любой момент)
        /// </summary>
        public void SetCryptoMetadata 
                (ReadOnlyMemory<byte>? nonceData, ReadOnlyMemory<byte>? tagData) {
            lock (lockObj) {
                // Устанавливаем только если еще не установлены
                if (nonce == null && nonceData.HasValue && nonceData.Value.Length > 0) {
                    nonce = nonceData.Value.ToArray();
                }

                if (tag == null && tagData.HasValue && tagData.Value.Length > 0) {
                    tag = tagData.Value.ToArray();
                }
            }
        }
        public byte[] Assemble () {
            for (int i = 0; i < parts.Length; i++) {
                if (parts[i] == null) {
                    throw new InvalidOperationException($"Fragment {i} is missing");
                }
            }
            var total = parts.Sum(p => p!.Value.Length);
            var result = new byte[total];
            int offset = 0;
            foreach (var part in parts) {
                part!.Value.CopyTo(result.AsMemory(offset));
                offset += part.Value.Length;
            }
            return result;
        }
    }
}
