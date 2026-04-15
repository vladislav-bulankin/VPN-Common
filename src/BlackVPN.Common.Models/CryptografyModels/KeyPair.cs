namespace BlackProjects.Common.CryptografyModels; 
public class KeyPair {
    /// <summary>
    /// Приватный ключ (держать в секрете)
    /// </summary>
    public byte[] PrivateKey { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Публичный ключ (отправлять другой стороне)
    /// </summary>
    public byte[] PublicKey { get; set; } = Array.Empty<byte>();
}
