namespace BlackProjects.Common.Enums; 
public enum AuthFailureReason {
    InvalidToken,
    TokenExpired,
    InvalidAudience,
    ReplayDetected,
    SessionLimitExceeded,
    InvalidSignature,
    InternalError
}
