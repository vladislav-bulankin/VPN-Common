namespace BlackProjects.Common.Packets;

/// <summary>
/// Пакет Pong
/// </summary>
public class PingPacket {
    /// <summary>
    /// Timestamp отправки
    /// </summary>
    public long Timestamp {
        get; set;
    }

    /// <summary>
    /// ID ping запроса
    /// </summary>
    public uint PingId {
        get; set;
    }
}
