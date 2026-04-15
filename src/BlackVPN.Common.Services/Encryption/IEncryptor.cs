using BlackProjects.Common.Encrypt;

namespace BlackProjects.Common.Abstractions.Encryption;

/// <summary>
/// Интерфейс шифрования данных
/// </summary>
public interface IEncryptor {
    /// <summary>
    /// Зашифровать данные
    /// </summary>
    /// <param name="plaintext">Открытый текст</param>
    /// <param name="key">Ключ шифрования</param>
    /// <param name="nonce">Nonce/IV (опционально, генерируется если null)</param>
    /// <returns>Зашифрованные данные с nonce</returns>
    Task<EncryptedData> EncryptAsync(byte[] plaintext, byte[] key, byte[]? nonce = null);

    /// <summary>
    /// Расшифровать данные
    /// </summary>
    /// <param name="encryptedData">Зашифрованные данные с nonce</param>
    /// <param name="key">Ключ шифрования</param>
    /// <returns>Открытый текст</returns>
    Task<byte[]> DecryptAsync(EncryptedData encryptedData, byte[] key);

    /// <summary>
    /// Размер ключа в байтах
    /// </summary>
    int KeySize {
        get;
    }

    /// <summary>
    /// Размер nonce в байтах
    /// </summary>
    int NonceSize {
        get;
    }
}
