namespace BlackProjects.Common.Models.Packets; 
public class ReassembledMessage {
    public byte[] Ciphertext { get; init; } = null!;
    public byte[] Nonce { get; init; } = null!;
    public byte[] Tag { get; init; } = null!;
}
