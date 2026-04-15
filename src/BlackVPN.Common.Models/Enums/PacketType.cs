namespace BlackProjects.Common.Enums;

/// <summary>
/// Типы VPN пакетов
/// </summary>
public enum PacketType {
    /// <summary>
    /// Запрос на подключение (handshake start)
    /// </summary>
    ConnectRequest = 0x01,

    /// <summary>
    /// Ответ на запрос подключения
    /// </summary>
    ConnectResponse = 0x02,

    /// <summary>
    /// Аутентификация (username/password)
    /// </summary>
    Authentication = 0x03,

    /// <summary>
    /// Результат аутентификации
    /// </summary>
    AuthenticationResult = 0x04,

    /// <summary>
    /// Обмен ключами (ECDH public key)
    /// </summary>
    KeyExchange = 0x05,

    /// <summary>
    /// Подтверждение обмена ключами
    /// </summary>
    KeyExchangeAck = 0x06,

    /// <summary>
    /// Пакет данных (зашифрованный туннелированный трафик)
    /// </summary>
    Data = 0x10,

    /// <summary>
    /// Keepalive (проверка соединения)
    /// </summary>
    Keepalive = 0x20,

    /// <summary>
    /// Ответ на keepalive
    /// </summary>
    KeepaliveAck = 0x21,

    /// <summary>
    /// Ping запрос (измерение RTT)
    /// </summary>
    Ping = 0x30,

    /// <summary>
    /// Pong ответ
    /// </summary>
    Pong = 0x31,

    /// <summary>
    /// Отключение
    /// </summary>
    Disconnect = 0xF0,

    /// <summary>
    /// Ошибка
    /// </summary>
    Error = 0xFF
}
