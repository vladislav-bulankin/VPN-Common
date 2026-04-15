namespace BlackProjects.Common.Packets;

/// <summary>
/// Пакет данных (зашифрованный IP пакет)
/// </summary>
public class DataPacket {
    public Guid PacketId { get; set; }          // один на весь IP-пакет
    public ushort FragmentIndex { get; set; }  // 0..N-1
    public ushort FragmentCount { get; set; }   // N
    /// <summary>
    /// Зашифрованные данные IP пакета
    /// </summary>
    public byte[] EncryptedData { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Nonce для расшифровки
    /// </summary>
    public byte[] Nonce { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Authentication tag (для AEAD)
    /// </summary>
    public byte[] Tag { get; set; } = Array.Empty<byte>();
}
