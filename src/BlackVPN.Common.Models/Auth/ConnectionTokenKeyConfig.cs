namespace BlackProjects.Common.Auth; 
public class ConnectionTokenKeyConfig {
    public string Id { get; set; } = null!;
    public bool isValid { get; set; }
    public string Type { get; set; } = null!;
    public string Secret { get; set; } = null!;
}
