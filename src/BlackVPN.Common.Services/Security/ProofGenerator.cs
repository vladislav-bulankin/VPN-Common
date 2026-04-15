using System.Security.Cryptography;
using System.Text;

namespace BlackProjects.Common.Services.Security;

/// <summary>
/// Генератор proof для защиты от активного зондирования
/// </summary>
public class ProofGenerator {
    /// <summary>
    /// Вычисляет SHA256 хэш токена
    /// </summary>
    public static byte[] ComputeTokenHash (string connectionToken) {
        return SHA256.HashData(Encoding.UTF8.GetBytes(connectionToken));
    }

    /// <summary>
    /// Извлекает signature из JWT токена (формат: header.payload.signature)
    /// </summary>
    public static byte[] ExtractSignature (string connectionToken) {
        var parts = connectionToken.Split('.');
        if (parts.Length != 3) {
            throw new ArgumentException(
                "Invalid token format (expected: header.payload.signature)",
                nameof(connectionToken)
            );
        }
        return Convert.FromBase64String(parts[2]);
    }

    /// <summary>
    /// Генерирует proof: HMAC-SHA256(signature, timestamp + challenge)
    /// Используется клиентом для доказательства владения токеном
    /// </summary>
    public static byte[] GenerateProof (
            string connectionToken,
            long timestamp,
            byte[] serverChallenge) {
        var signature = ExtractSignature(connectionToken);
        // Данные: timestamp (8 bytes) + challenge
        var data = new byte[8 + serverChallenge.Length];
        BitConverter.GetBytes(timestamp).CopyTo(data, 0);
        serverChallenge.CopyTo(data, 8);
        // HMAC-SHA256 с signature как ключом
        using var hmac = new HMACSHA256(signature);
        return hmac.ComputeHash(data);
    }

    /// <summary>
    /// Валидирует proof на сервере
    /// Используется узлом для проверки легитимности клиента
    /// </summary>
    public static bool ValidateProof (
            byte[] proof,
            byte[] tokenSignature,
            long timestamp,
            byte[] serverChallenge,
            TimeSpan? maxAge = null) {
        maxAge ??= TimeSpan.FromMinutes(5);
        // 1. Проверка timestamp (защита от replay атак)
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var age = Math.Abs(now - timestamp);
        if (age > maxAge.Value.TotalSeconds) { return false; }
        // 2. Вычисляем ожидаемый proof
        var data = new byte[8 + serverChallenge.Length];
        BitConverter.GetBytes(timestamp).CopyTo(data, 0);
        serverChallenge.CopyTo(data, 8);
        byte[] expectedProof;
        try {
            using var hmac = new HMACSHA256(tokenSignature);
            expectedProof = hmac.ComputeHash(data);
        } catch { return false; }
        // 3. Constant-time сравнение (защита от timing атак)
        return CryptographicOperations.FixedTimeEquals(proof, expectedProof);
    }

    /// <summary>
    /// Генерирует случайный challenge (16 байт)
    /// Может использоваться для динамических challenge (не обязательно)
    /// </summary>
    public static byte[] GenerateChallenge () {
        return RandomNumberGenerator.GetBytes(16);
    }
}
