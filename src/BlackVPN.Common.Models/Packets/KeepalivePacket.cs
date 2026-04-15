namespace BlackProjects.Common.Packets;

/// <summary>
/// Пакет Keepalive
/// </summary>
public class KeepalivePacket {
    /// <summary>
    /// Timestamp для измерения времени
    /// </summary>
    public long Timestamp {
        get; set;
    }
}
