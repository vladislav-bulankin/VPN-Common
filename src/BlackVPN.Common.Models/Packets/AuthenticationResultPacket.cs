namespace BlackProjects.Common.Packets;

/// <summary>
/// Результат аутентификации
/// </summary>
public class AuthenticationResultPacket {
    /// <summary>
    /// Успешна ли аутентификация
    /// </summary>
    public bool Success {
        get; set;
    }

    public Guid SessionId {
        get; set;
    }

    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Виртуальный IP адрес (если успешно)
    /// </summary>
    public string? VirtualIpAddress {
        get; set;
    }

    /// <summary>
    /// DNS серверы
    /// </summary>
    public string[] DnsServers { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Перенаправлять ли весь трафик через VPN
    /// </summary>
    public bool RedirectGateway {
        get; set;
    }
}
