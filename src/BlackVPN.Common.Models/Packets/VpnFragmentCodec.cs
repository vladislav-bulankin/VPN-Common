using System.Buffers.Binary;

namespace BlackProjects.Common.Models.Packets; 
public class VpnFragmentCodec {
    private const int HeaderSize = 9; // 4 + 2 + 2 + 1 

    /// <summary>
    /// Encode фрагмента
    /// nonce/tag передаются ТОЛЬКО для First-фрагмента  
    /// </summary>
    public static byte[] Encode (
            in VpnFragmentHeader header,
            ReadOnlySpan<byte> payload,
            ReadOnlySpan<byte> nonce = default,
            ReadOnlySpan<byte> tag = default) {
        bool isFirst = header.Flags.HasFlag(VpnFragmentFlags.First);
        int extraSize = isFirst ? (nonce.Length + tag.Length) : 0;
        var buffer = new byte[HeaderSize + extraSize + payload.Length];
        var span = buffer.AsSpan();
        BinaryPrimitives.WriteUInt32LittleEndian(span[0..4], header.MessageId);
        BinaryPrimitives.WriteUInt16LittleEndian(span[4..6], header.FragmentIndex);
        BinaryPrimitives.WriteUInt16LittleEndian(span[6..8], header.FragmentCount);
        span[8] = (byte)header.Flags;
        int offset = HeaderSize;
        if (isFirst) {
            nonce.CopyTo(span[offset..]);
            offset += nonce.Length;
            tag.CopyTo(span[offset..]);
            offset += tag.Length;
        }
        payload.CopyTo(span[offset..]);
        return buffer;
    }

    /// <summary>
    /// Decode фрагмента
    /// </summary>
    public static 
            (VpnFragmentHeader header, ReadOnlyMemory<byte> nonce, ReadOnlyMemory<byte> tag, ReadOnlyMemory<byte> payload)
                Decode (
                     ReadOnlyMemory<byte> buffer,
                     int nonceLength,
                     int tagLength) {
        var span = buffer.Span;
        var header = new VpnFragmentHeader(
            BinaryPrimitives.ReadUInt32LittleEndian(span[0..4]),
            BinaryPrimitives.ReadUInt16LittleEndian(span[4..6]),
            BinaryPrimitives.ReadUInt16LittleEndian(span[6..8]),
            (VpnFragmentFlags)span[8]
        );
        int offset = HeaderSize;
        ReadOnlyMemory<byte> nonce = ReadOnlyMemory<byte>.Empty;
        ReadOnlyMemory<byte> tag = ReadOnlyMemory<byte>.Empty;
        if (header.Flags.HasFlag(VpnFragmentFlags.First)) {
            nonce = buffer.Slice(offset, nonceLength);
            offset += nonceLength;
            tag = buffer.Slice(offset, tagLength);
            offset += tagLength;
        }
        var payload = buffer[offset..];
        return (header, nonce, tag, payload);
    }
}
