namespace BlackProjects.Common.Packets;

/// <summary>
/// Ответ на запрос подключения
/// </summary>
public class ConnectResponsePacket {
    public Guid HandshakeId {
        get; init;
    }

    /// <summary>
    /// Сообщение (причина отказа если не принят)
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// отказано или принето
    /// </summary>
    public bool? Accepted {
        get;
        set;
    }

    /// <summary>
    /// Публичный ключ сервера для ECDH
    /// </summary>
    public byte[] ServerPublicKey { get; set; } = Array.Empty<byte>();
    public byte[] Challenge { get; set; } = Array.Empty<byte>();
}
