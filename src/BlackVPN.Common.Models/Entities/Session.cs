namespace BlackProjects.Common.Entities;

/// <summary>
/// Сущность VPN сессии
/// </summary>
public class Session {
    public Guid Id {
        get; set;
    }

    public Guid UserId {
        get; set;
    }

    public string VirtualIpAddress { get; set; } = string.Empty;

    public string ClientIpAddress { get; set; } = string.Empty;

    public int ClientPort {
        get; set;
    }

    public int Status { get; set; } = 0;// Connecting;

    /// <summary>
    /// Ключ шифрования сессии (симметричный ключ)
    /// </summary>
    public byte[] SessionKey { get; set; } = Array.Empty<byte>();

    public int EncryptionAlgorithm { get; set; } = 0;//AES256

    /// <summary>
    /// Время начала сессии
    /// </summary>
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Время последней активности
    /// </summary>
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Время отключения (null если активна)
    /// </summary>
    public DateTime? DisconnectedAt {
        get; set;
    }

    /// <summary>
    /// Отправлено байт
    /// </summary>
    public long BytesSent { get; set; } = 0;

    /// <summary>
    /// Получено байт
    /// </summary>
    public long BytesReceived { get; set; } = 0;

    /// <summary>
    /// Общий трафик сессии
    /// </summary>
    public long TotalTraffic => BytesSent + BytesReceived;

    /// <summary>
    /// Причина отключения
    /// </summary>
    public string? DisconnectReason {
        get; set;
    }
}
