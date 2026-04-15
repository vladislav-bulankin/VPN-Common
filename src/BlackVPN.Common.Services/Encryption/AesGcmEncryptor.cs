using BlackProjects.Common.Abstractions.Encryption;
using BlackProjects.Common.Encrypt;
using System.Security.Cryptography;


namespace BlackProjects.Common.Encryption;

/// <summary>
/// AES-256-GCM шифрование
/// </summary>
public class AesGcmEncryptor : IEncryptor {
    private readonly byte[] key;
    public int KeySize => 32; // 256 бит
    public int NonceSize => 12; // 96 бит (рекомендуемый размер для GCM)
    private const int TagSize = 16; // 128 бит
    public AesGcmEncryptor(byte[] key) {
        if(key == null || (key.Length != 16 && key.Length != 24 && key.Length != 32))
            throw new ArgumentException("Key must be 128, 192, or 256 bits.", nameof(key));

        this.key = key;
    }

    public AesGcmEncryptor() {
    }

    /// <summary>
    /// Расшифровать
    /// </summary>
    /// <param name="encryptedData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<byte[]> DecryptAsync(EncryptedData encryptedData, byte[] key) {
        if(encryptedData == null) {
            throw new ArgumentNullException(nameof(encryptedData));
        }
        if(key == null || key.Length != KeySize) {
            throw new ArgumentException($"Key must be {KeySize} bytes", nameof(key));
        }
        if(encryptedData.Nonce.Length != NonceSize) {
            throw new ArgumentException($"Nonce must be {NonceSize} bytes");
        }
        if(encryptedData.Tag.Length != TagSize) {
            throw new ArgumentException($"Tag must be {TagSize} bytes");
        }
        return await Task.Run(() => {
            var plaintext = new byte[encryptedData.Ciphertext.Length];
            using var aesGcm = new AesGcm(key, TagSize);
            aesGcm.Decrypt(
                encryptedData.Nonce,
                encryptedData.Ciphertext,
                encryptedData.Tag,
                plaintext);
            return plaintext;
        });
    }

    /// <summary>
    /// Зашифровать
    /// </summary>
    /// <param name="plaintext"></param>
    /// <param name="key"></param>
    /// <param name="nonce"></param>
    /// <returns></returns>
    public async Task<EncryptedData>
            EncryptAsync(byte[] plaintext, byte[] key, byte[]? nonce = null) {
        if(plaintext == null || plaintext.Length == 0) {
            throw new ArgumentException("Plaintext cannot be empty", nameof(plaintext));
        }
        if(key == null || key.Length != KeySize) {
            throw new ArgumentException($"Key must be {KeySize} bytes", nameof(key));
        }
        if(nonce == null) {
            nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(nonce);
        }
        if(nonce.Length != NonceSize) {
            throw new ArgumentException($"Nonce must be {NonceSize} bytes", nameof(nonce));
        }
        return await Task.Run(() => {
            var ciphertext = new byte[plaintext.Length];
            var tag = new byte[TagSize];
            using var aesGcm = new AesGcm(key, TagSize);
            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);
            return new EncryptedData {
                Ciphertext = ciphertext,
                Nonce = nonce,
                Tag = tag
            };
        });
    }
}
