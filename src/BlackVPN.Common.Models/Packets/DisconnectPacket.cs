namespace BlackProjects.Common.Packets; 
public class DisconnectPacket {
    /// <summary>
    /// Причина отключения
    /// </summary>
    public string Reason { get; set; } = string.Empty;
}
