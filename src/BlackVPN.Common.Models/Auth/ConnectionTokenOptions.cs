using BlackProjects.Common.Auth;

namespace BlackProjects.Common.Models.Auth; 
public class ConnectionTokenOptions {
    public ConnectionTokenKeyConfig[] Keys { get; set; } 
        = Array.Empty<ConnectionTokenKeyConfig>();
}
