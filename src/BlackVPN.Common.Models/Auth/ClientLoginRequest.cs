using System.ComponentModel.DataAnnotations;

namespace BlackProjects.Common.Models.Auth; 
public class ClientLoginRequest {
    [Required]
    public string? UserNameOrEmail {
        get; set;
    }
    [Required]
    public string? Password {
        get; set;
    }
}
