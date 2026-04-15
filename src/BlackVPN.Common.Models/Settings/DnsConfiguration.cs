namespace BlackProjects.Common.Settings;

/// <summary>
/// Конфигурация DNS для VPN клиентов
/// </summary>
public class DnsConfiguration {
    /// <summary>
    /// Основные DNS серверы
    /// </summary>
    public string[] DnsServers { get; set; } = new[] { "8.8.8.8", "8.8.4.4" };

    /// <summary>
    /// Резервные DNS серверы (используются при ротации)
    /// </summary>
    public string[] BackupDnsServers { get; set; } = new[] { "1.1.1.1", "1.0.0.1" };

    /// <summary>
    /// Перенаправлять весь трафик через VPN
    /// </summary>
    public bool RedirectGateway { get; set; } = true;

    /// <summary>
    /// Автоматическая ротация DNS при DDoS
    /// </summary>
    public bool RotateDnsOnDdos { get; set; } = false;

    /// <summary>
    /// Интервал ротации DNS в минутах
    /// </summary>
    public int DnsRotationIntervalMinutes { get; set; } = 5;

    /// <summary>
    /// Использовать резервные DNS
    /// </summary>
    public bool UseBackupDns { get; set; } = false;

    /// <summary>
    /// Кастомные DNS маршруты (domain -> DNS IP)
    /// </summary>
    public Dictionary<string, string>? CustomDnsRoutes {
        get; set;
    }

    /// <summary>
    /// Получить активные DNS серверы (с учетом ротации)
    /// </summary>
    public string[] GetActiveDnsServers() {
        return UseBackupDns ? BackupDnsServers : DnsServers;
    }
}
