using BlackProjects.Common.Enums;

namespace BlackProjects.Common.Auth; 
public class SessionContext {
    public Guid SessionId {
        get; set;
    }
    public Guid UserId {
        get; set;
    }
    public string VirtualIp { get; set; } = null!;
    public int Mtu {
        get; set;
    }
    public DateTime ConnectedAt {
        get;
        set;
    }
    public DateTime LastActivityAt {
        get;
        set;
    }
    public Guid NodeId {
        get;
        set;
    }
    public DateTimeOffset IssuedAt {
        get; set;
    }
    public ConnectionStatus Status {
        get;
        set;
    }
    public SessionLimits Limits { get; set; } = null!;
}
