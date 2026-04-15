namespace BlackProjects.Common.Packets;

/// <summary>
/// Запрос на подключение
/// </summary>
public class ConnectRequestPacket {
    /// <summary>
    /// Публичный ключ клиента для ECDH
    /// </summary>
    public byte[] ClientPublicKey { get; set; } = Array.Empty<byte>();
}
