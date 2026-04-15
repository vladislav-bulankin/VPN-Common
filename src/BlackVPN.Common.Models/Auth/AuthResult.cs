using BlackProjects.Common.Enums;

namespace BlackProjects.Common.Auth; 
public class AuthResult {
    public bool IsAuthenticated {
        get; init;
    }
    public AuthFailureReason? FailureReason {
        get; init;
    }
    public SessionContext? Session {
        get; init;
    }
}
