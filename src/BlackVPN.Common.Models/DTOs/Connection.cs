using BlackProjects.Common.Enums;
using System.Net;
using System.Security.Cryptography;

namespace BlackProjects.Common.DTOs;

/// <summary>
/// Активное подключение (in-memory, не хранится в БД постоянно)
/// Представляет текущее состояние подключения
/// </summary>
public class Connection {
    public Connection (
        Guid sessionId,
        Guid userId,
        IPEndPoint endpoint,
        byte[] key,
        EncryptionAlgorithm algorithm
    ) {
        SessionId = sessionId;
        ClientEndpoint = endpoint;
        EncryptionKey = key;
        EncryptionAlgorithm = algorithm;
        UserId = userId;
        Touch();
    }
    public Connection () { }
    public Guid SessionId {
        get; set;
    }
    public Guid UserId {
        get; set;
    }
    /// <summary>
    /// IP и порт
    /// </summary>
    public IPEndPoint? ClientEndpoint { get; set; } = null;
    /// <summary>
    /// Время последнего пакета
    /// </summary>
    public DateTime LastPacketTime { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Счетчик отправленных пакетов
    /// </summary>
    public long PacketsSent { get; set; } = 0;

    /// <summary>
    /// Счетчик полученных пакетов
    /// </summary>
    public long PacketsReceived { get; set; } = 0;

    /// <summary>
    /// Отправлено байт в текущей сессии
    /// </summary>
    public long BytesSent { get; set; } = 0;

    /// <summary>
    /// Получено байт в текущей сессии
    /// </summary>
    public long BytesReceived { get; set; } = 0;

    /// <summary>
    /// Время создания подключения
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Пинг (RTT) в миллисекундах
    /// </summary>
    public int PingMs { get; set; } = 0;

    /// <summary>
    /// Активно ли подключение
    /// </summary>
    public bool IsActive => (DateTime.UtcNow - LastPacketTime).TotalSeconds < 30;

    public ulong LastReceivedSequence {
        get; set;
    }
    public ulong NextSendSequence {
        get; set;
    }

    public byte[]? EncryptionKey {
        get; private set;
    }
    public void Touch () =>
        LastPacketTime = DateTime.UtcNow;

    public EncryptionAlgorithm EncryptionAlgorithm {
        get; init;
    }

    public void GenerateEncryptionKey() {
        EncryptionKey = new byte[32];
        RandomNumberGenerator.Fill(EncryptionKey);
    }

    public void SetEncryptionKey(byte[] key) {
        if(key.Length != 32) {
            throw new ArgumentException("Encryption key must be 32 bytes");
        }
        EncryptionKey = new byte[32];
        Buffer.BlockCopy(key, 0, EncryptionKey, 0, 32);
    }

    /// <summary>
    /// Окно для out-of-order пакетов (опционально)
    /// </summary>
    public HashSet<ulong> RecentSequences = new(capacity: 128);

    public long PacketsInCurrentWindow {
        get; set;
    } = 0;
    public DateTime WindowStartTime {
        get; set;
    } = DateTime.UtcNow;
    public const int MAX_PACKETS_PER_SECOND = 1000; // настраиваемо
    private readonly object _sequenceLock = new();
    private readonly object _rateLimitLock = new();

    /// <summary>
    /// Валидация пакета на replay ?
    /// </summary>
    public bool ValidateSequence(ulong sequenceNumber) {
        lock(_sequenceLock) {
            // Пакет слишком старый
            if(sequenceNumber < LastReceivedSequence - 100) {
                return false;
            }

            // Пакет уже был обработан
            if(RecentSequences.Contains(sequenceNumber)) {
                return false;
            }

            // Добавляем в окно и обновляем последний
            RecentSequences.Add(sequenceNumber);
            if(sequenceNumber > LastReceivedSequence) {
                LastReceivedSequence = sequenceNumber;
            }

            // Чистим старые sequence numbers
            if(RecentSequences.Count > 100) {
                var toRemove = RecentSequences
                    .Where(s => s < LastReceivedSequence - 100)
                    .ToList();
                foreach(var s in toRemove) {
                    RecentSequences.Remove(s);
                }
            }

            return true;
        }
    }

    public int MaxPacketsPerSecond { get; set; } = 1000;

    public ulong GetNextSendSequence() {
        lock(this) {
            return NextSendSequence++;
        }
    }

    public bool CheckRateLimit() {
        lock(_rateLimitLock) {
            var now = DateTime.UtcNow;
            var windowDuration = TimeSpan.FromSeconds(1);

            // Сброс окна каждую секунду
            if((now - WindowStartTime) >= windowDuration) {
                WindowStartTime = now;
                PacketsInCurrentWindow = 0;
            }

            PacketsInCurrentWindow++;

            return PacketsInCurrentWindow <= MaxPacketsPerSecond;
        }
    }
}
