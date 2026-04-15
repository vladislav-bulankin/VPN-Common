namespace BlackProjects.Common.Auth; 
public class ClientLoginResponse {
    /// <summary>
    /// Успешна ли аутентификация
    /// </summary>
    public bool Success { get; init; }
       
   

    public string? AccessToken { get; init; }
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Сообщение об ошибке (если Success == false)
    /// </summary>
    public string? Error { get; init; }
       
   
}
