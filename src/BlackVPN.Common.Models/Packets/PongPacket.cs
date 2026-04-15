namespace BlackProjects.Common.Packets;

/// <summary>
/// Пакет Pong
/// </summary>
public class PongPacket {
    /// <summary>
    /// Timestamp из ping запроса
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
