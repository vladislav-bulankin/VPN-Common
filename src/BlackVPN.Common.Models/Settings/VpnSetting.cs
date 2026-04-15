using BlackProjects.Common.Enums;

namespace BlackProjects.Common.Settings;

/// <summary>
/// Конфигурация VPN сервера
/// Хранится в БД для динамической настройки
/// </summary>
public class VpnSetting {
    public Guid Id { get; set; }

    /// <summary>
    /// Название конфигурации
    /// </summary>
    public string Name { get; set; } = "vpntun0";

    public int ServerPort { get; set; } = 5000;
    public int TcpPort { get; set; } = 8443;  // дефолтные значения
    public int UdpPort { get; set; } = 8443;
    public string? ServerIpAddress { get; set; }
    public string? ServerRegion { get; set; }
    /// <summary>
    /// TCP (true) UDP (false)
    /// </summary>
    public bool UseTcp { get; set; } = false;

    /// <summary>
    /// Подсеть для виртуальных IP (например, "10.8.0.0/16")
    /// </summary>
    public string VirtualSubnet { get; set; } = "10.8.0.0/16";

    /// <summary>
    /// DNS серверы для клиентов (разделенные запятыми)
    /// </summary> 
    public string DnsServers { get; set; } = "8.8.8.8,8.8.4.4,1.1.1.1,1.0.0.1";

    /// <summary>
    /// Максимальное количество одновременных подключений
    /// </summary>
    public int MaxConnections { get; set; } = 1000;

    /// <summary>
    /// Таймаут неактивности (секунды)
    /// </summary>
    public int IdleTimeoutSeconds { get; set; } = 300;

    /// <summary>
    /// Интервал keepalive (секунды)
    /// </summary>
    public int KeepaliveIntervalSeconds { get; set; } = 10;

    /// <summary>
    /// Алгоритм шифрования по умолчанию
    /// </summary>
    public EncryptionAlgorithm DefaultEncryption { get; set; } = EncryptionAlgorithm.AES256;

    /// <summary>
    /// Включить сжатие данных
    /// </summary>
    public bool CompressionEnabled { get; set; } = false;

    /// <summary>
    /// Маршруты для пробрасывания клиентам (JSON array)
    /// </summary>
    public string? Routes { get; set; }

    /// <summary>
    /// Перенаправлять весь трафик через VPN
    /// </summary>
    public bool RedirectGateway { get; set; } = true;

    /// <summary>
    /// Включен ли логирование подключений
    /// </summary>
    public bool ConnectionLoggingEnabled { get; set; } = true;

    /// <summary>
    /// Активна ли эта конфигурация
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Maximum Transmission Unit
    /// максимальный размер пакета (в байтах), 
    /// который можно передать без фрагментации.
    /// </summary>
    public int Mtu { get; set; } = 1400;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
