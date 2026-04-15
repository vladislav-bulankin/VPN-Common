using System.ComponentModel.DataAnnotations;

namespace BlackProjects.Common.Auth; 
public class ConnectionTokenRequest {
    [Required]//connect for defender mail storedg... products NodeId = null or guid,empty
    public Guid? NodeId { get; set; }
    [Required]
    public string? ProductId { get; set; }
}
