namespace BlackProjects.Common.Packets; 
public class AuthenticationPacket {
    public Guid HandshakeId {
        get; init;
    }
    public byte[] ConnectionToken { get; init; } = null!;
    public byte[] Proof { get; set; } = Array.Empty<byte>();
    public long Timestamp { get; set; }
}
