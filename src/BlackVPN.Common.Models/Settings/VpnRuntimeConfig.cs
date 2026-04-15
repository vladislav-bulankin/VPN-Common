namespace BlackProjects.Common.Settings; 
public class VpnRuntimeConfig {
    public string Name { get; set; } = default!;
    public int ServerPort {
        get; set;
    }
    public string? ServerIpAddress {
        get;
        set;
    }
    public bool UseTcp {
        get; set;
    }
    public string VirtualSubnet { get; set; } = default!;
    public string[] DnsServers { get; set; } = [];
    public int MaxConnections {
        get; set;
    }
    public int IdleTimeoutSeconds {
        get; set;
    }
    public int KeepaliveIntervalSeconds {
        get; set;
    }
    public string? DefaultEncryption {
        get; set;
    }
    public bool CompressionEnabled {
        get; set;
    }
    public string? Routes {
        get; set;
    }
    public bool RedirectGateway {
        get; set;
    }
    public bool ConnectionLoggingEnabled {
        get; set;
    }

    public int Mtu {
        get; set;
    }
    public bool IsActive {
        get; set;
    }
}
