namespace BlackProjects.Common.Auth; 
public class ConnectionTokenPayload {
    // === Identity ===
    public Guid TokenId { get; set; }          // jti

    public Guid UserId { get; set; }           // sub

    public Guid SessionId { get; set; }        // sid
    public Guid ProductId { get; set; }

    // === Audience ===
    public string Audience { get; set; } = null!; // nodeId

    // === Lifetime ===
    public DateTimeOffset IssuedAt { get; set; }   // iat

    public DateTimeOffset ExpiresAt { get; set; } // exp
       
    // === Limits ===
    public SessionLimits Limits { get; set; } = null!;
}

public class TokenHeader {
    public string KeyId { get; set; } = null!;
    public string Algorithm { get; set; } = "HS256";
}

public class TokenSigningKey {
    public string Id { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Secret { get; set; } = null!;
}