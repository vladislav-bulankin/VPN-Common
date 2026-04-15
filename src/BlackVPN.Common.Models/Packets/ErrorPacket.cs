namespace BlackProjects.Common.Packets;

/// <summary>
/// Пакет ошибки
/// </summary>
public class ErrorPacket {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public uint ErrorCode {
        get; set;
    }

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
