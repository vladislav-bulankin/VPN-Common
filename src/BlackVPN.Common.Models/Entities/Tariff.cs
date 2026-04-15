namespace BlackProjects.Common.Entities; 
public class Tariff {
    public Guid Id {
        get; set;
    }
    public string Name { get; set; } = string.Empty;
    public string? Description {
        get; set;
    }
    public long? TrafficLimit {
        get; set;
    }
    public int MaxConnections {
        get; set;
    }
    public int DurationDays {
        get; set;
    }
    public decimal Price {
        get; set;
    }
    public bool IsActive {
        get; set;
    }
    public DateTime CreatedAt {
        get; set;
    }
    public DateTime? UpdatedAt {
        get; set;
    }
}
