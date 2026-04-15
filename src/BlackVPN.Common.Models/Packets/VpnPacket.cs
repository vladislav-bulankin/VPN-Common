using BlackProjects.Common.Enums;
using System.Buffers.Binary;

namespace BlackProjects.Common.Packets;

/// <summary>
/// Базовый VPN пакет
/// Формат: [Type(1)][SessionId(16)][SequenceNumber(4)][PayloadLength(4)][Payload(N)]
/// </summary>
public class VpnPacket {
    /// <summary>
    /// Тип пакета
    /// </summary>
    public PacketType Type {
        get; set;
    }

    /// <summary>
    /// ID сессии (16 байт - Guid)
    /// </summary>
    public Guid SessionId {
        get; set;
    }

    /// <summary>
    /// Порядковый номер пакета (для защиты от replay attacks)
    /// </summary>
    public uint SequenceNumber {
        get; set;
    }

    /// <summary>
    /// Полезная нагрузка
    /// </summary>
    public byte[] Payload { get; set; } = Array.Empty<byte>();


    // Размеры полей в байтах
    public const int TypeSize = 1;
    public const int SessionIdSize = 16;
    public const int SequenceNumberSize = 4;
    public const int PayloadLengthSize = 4;
    public const int HeaderSize =
        TypeSize
        + SessionIdSize
        + SequenceNumberSize
        + PayloadLengthSize;

    // Для сериализации в байты (отправка по сети)
    public byte[] ToBytes() {
        var buffer = new byte[HeaderSize + Payload.Length];
        var span = buffer.AsSpan();
        span[0] = (byte)Type;
        SessionId.TryWriteBytes(span.Slice(1, 16));
        BinaryPrimitives.WriteUInt32BigEndian(
            span.Slice(17, 4),
            SequenceNumber
        );
        BinaryPrimitives.WriteInt32BigEndian(
            span.Slice(21, 4),
            Payload.Length
        );
        Payload.CopyTo(span.Slice(25));
        return buffer;
    }

    // Для десериализации из байтов (прием из сети)
    public static VpnPacket FromBytes(byte[] data) {
        if(data.Length < HeaderSize){
            throw new InvalidOperationException("Packet too short");
        }
        var dataSpan = data.AsSpan();
        var type = (PacketType)dataSpan[0];
        var sessionId = new Guid(dataSpan.Slice(1, 16));
        var sequence = BinaryPrimitives.ReadUInt32BigEndian(
            dataSpan.Slice(17, 4)
        );

        var payloadLength = BinaryPrimitives.ReadInt32BigEndian(
            dataSpan.Slice(21, 4)
        );

        if(dataSpan.Length < HeaderSize + payloadLength){
            throw new InvalidOperationException("Invalid payload length");
        }

        var payload = dataSpan.Slice(25, payloadLength).ToArray();

        return new VpnPacket {
            Type = type,
            SessionId = sessionId,
            SequenceNumber = sequence,
            Payload = payload
        };
    }
}
