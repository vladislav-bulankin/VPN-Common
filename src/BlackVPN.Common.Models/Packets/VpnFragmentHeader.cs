namespace BlackProjects.Common.Models.Packets;

public class VpnFragmentHeader {

    public uint MessageId { get; }
    public ushort FragmentIndex { get; }
    public ushort FragmentCount { get; }
    public VpnFragmentFlags Flags { get; }

    public VpnFragmentHeader (
            uint messageId,
            ushort fragmentIndex,
            ushort fragmentCount,
            VpnFragmentFlags flags = VpnFragmentFlags.None) {
        MessageId = messageId;
        FragmentIndex = fragmentIndex;
        FragmentCount = fragmentCount;
        Flags = flags;
    }
}

[Flags]
public enum VpnFragmentFlags : byte {
    None = 0,
    First = 1
}