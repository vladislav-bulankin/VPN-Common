namespace BlackProjects.Common.Auth; 
public class SessionLimits {
    public int MaxConnections {
        get; init;
    }
    public long? TrafficLimitBytes {
        get; init;
    }
    public int IdleTimeoutSeconds {
        get; init;
    }
    public DateTimeOffset? DurationDate {
        get;
        set;
    }
}
