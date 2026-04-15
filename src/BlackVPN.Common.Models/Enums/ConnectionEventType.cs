namespace BlackProjects.Common.Enums;

/// <summary>
/// Тип события подключения
/// </summary>
public enum ConnectionEventType {
    /// <summary>
    /// Попытка подключения
    /// </summary>
    ConnectionAttempt = 0,

    /// <summary>
    /// Успешное подключение
    /// </summary>
    Connected = 1,

    /// <summary>
    /// Нормальное отключение
    /// </summary>
    Disconnected = 2,

    /// <summary>
    /// Отключение по таймауту
    /// </summary>
    Timeout = 3,

    /// <summary>
    /// Ошибка аутентификации
    /// </summary>
    AuthenticationFailed = 4,

    /// <summary>
    /// Ошибка подключения
    /// </summary>
    ConnectionError = 5,

    /// <summary>
    /// Принудительное отключение
    /// </summary>
    ForcedDisconnect = 6
}
