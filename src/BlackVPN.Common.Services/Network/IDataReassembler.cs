using BlackProjects.Common.Models.Packets;

namespace BlackProjects.Common.Services.Network; 
public interface IDataReassembler {
    void Reset (Guid sessionId);
    /// <summary>
    /// Добавляет фрагмент и возвращает собранный payload,
    /// либо null если сборка не завершена
    /// </summary>
    ReassembledMessage? Push (
            Guid sessionId,
            VpnFragmentHeader header,
            ReadOnlyMemory<byte> ciphertext,
            ReadOnlyMemory<byte>? nonce,
            ReadOnlyMemory<byte>? tag);
}
