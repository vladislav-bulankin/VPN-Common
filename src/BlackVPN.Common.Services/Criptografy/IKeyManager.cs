namespace BlackProjects.Common.Abstractions.Criptografy;

/// <summary>
/// Менеджер ключей - генерация и управление ключами
/// </summary>
public interface IKeyManager {
    /// <summary>
    /// Сгенерировать случайный ключ
    /// </summary>
    /// <param name="sizeInBytes">Размер ключа в байтах</param>
    /// <returns>Случайный ключ</returns>
    byte[] GenerateKey(int sizeInBytes);

    /// <summary>
    /// Сгенерировать nonce/IV
    /// </summary>
    /// <param name="sizeInBytes">Размер nonce в байтах</param>
    /// <returns>Случайный nonce</returns>
    byte[] GenerateNonce(int sizeInBytes);

    /// <summary>
    /// Вывести ключ из пароля (Key Derivation Function)
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <param name="salt">Соль</param>
    /// <param name="iterations">Количество итераций</param>
    /// <param name="keyLength">Длина ключа</param>
    /// <returns>Выведенный ключ</returns>
    byte[] DeriveKeyFromPassword
        (string password, byte[] salt, int iterations, int keyLength);

    byte[] DeriveSessionKey
        (byte[] sharedSecret, byte[] salt, string info, int keyLength = 32);
    void SecureEraseKey(byte[] key);
}
