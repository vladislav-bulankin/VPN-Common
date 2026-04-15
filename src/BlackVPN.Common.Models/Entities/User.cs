namespace BlackProjects.Common.Entities;

/// <summary>
/// Сущность пользователя VPN
/// </summary>
public class User {
    public Guid Id {
        get; set;
    }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public int Role { get; set; } = 0; // UserRole.User;

    public bool IsActive { get; set; } = true;

    public int MaxConnections { get; set; } = 1;

    /// <summary>
    /// Дата создания аккаунта
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime? UpdatedAt {
        get; set;
    }

    /// <summary>
    /// Дата последнего входа
    /// </summary>
    public DateTime? LastLoginAt {
        get; set;
    }

    /// <summary>
    /// Срок действия аккаунта (null = бессрочно)
    /// </summary>
    public DateTime? ExpiresAt {
        get; set;
    }

    /// <summary>
    /// Ограничение трафика в байтах (null = без ограничений)
    /// </summary>
    public long? TrafficLimit {
        get; set;
    }

    /// <summary>
    /// Использованный трафик в байтах
    /// </summary>
    public long TrafficUsed { get; set; } = 0;
}
