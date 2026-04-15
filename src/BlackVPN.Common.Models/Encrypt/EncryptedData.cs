namespace BlackProjects.Common.Encrypt;

/// <summary>
/// Зашифрованные данные с метаданными
/// </summary>
public class EncryptedData {
    public byte[] Ciphertext { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Nonce/IV использованный при шифровании
    /// </summary>
    public byte[] Nonce { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Authentication tag (для AEAD алгоритмов)
    /// </summary>
    public byte[] Tag { get; set; } = Array.Empty<byte>();
}
