using BlackProjects.Common.Enums;
using BlackProjects.Common.Packets;

using System.Text;

namespace BlackProjects.Common.Protocols;

/// <summary>
/// Декодировщик VPN пакетов из байтов
/// </summary>
public class ProtocolDecoder {
    /// <summary>
    /// Декодировать VPN пакет из байтов
    /// </summary>
    public VpnPacket Decode(byte[] data) {

        if(data.Length < VpnPacket.HeaderSize) {
            throw new ArgumentException("Data too short for VPN packet");
        }

        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);

        var packet = new VpnPacket {
            Type = (PacketType)reader.ReadByte(),
            SessionId = new Guid(reader.ReadBytes(VpnPacket.SessionIdSize)),
            SequenceNumber = reader.ReadUInt32()
        };

        var payloadLength = reader.ReadInt32();

        if(payloadLength > 0 && ms.Position + payloadLength <= ms.Length) {
            packet.Payload = reader.ReadBytes(payloadLength);
        }

        return packet;
    }

    /// <summary>
    /// Декодировать ConnectRequest
    /// </summary>
    public ConnectRequestPacket DecodeConnectRequest (byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);
        //Читаем длину ключа и сам ключ
        var keyLength = reader.ReadInt32();
        var publicKey = reader.ReadBytes(keyLength);
        return new ConnectRequestPacket {
            ClientPublicKey = publicKey
        };
    }

    /// <summary>
    /// Декодировать ConnectResponse
    /// </summary>
    public ConnectResponsePacket DecodeConnectResponse (byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);
        // HandshakeId (Важно: читаем первым!)
        var handshakeId = new Guid(reader.ReadBytes(16));
        // Accepted
        byte acceptedStatus = reader.ReadByte();
        bool? accepted = acceptedStatus switch {
            0 => false,
            1 => true,
            _ => null
        };
        // Message
        var message = ReadString(reader);
        //ServerPublicKey (Сначала длину, потом байты)
        var keyLength = reader.ReadInt32();
        var serverKey = reader.ReadBytes(keyLength);
        // Challenge
        var challengeLength = reader.ReadInt32();
        byte[]? challenge = null;
        if (challengeLength > 0) {
            challenge = reader.ReadBytes(challengeLength);
        }
        return new ConnectResponsePacket {
            HandshakeId = handshakeId,
            Accepted = accepted,
            Message = message,
            ServerPublicKey = serverKey,
            Challenge = challenge ?? Array.Empty<byte>()
        };
    }

    /// <summary>
    /// Декодировать Authentication
    /// </summary>
    public AuthenticationPacket DecodeAuthentication(byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);
        // HandshakeId
        var handshakeId = new Guid(reader.ReadBytes(16));
        // ConnectionToken
        var tokenLength = reader.ReadInt32();
        var token = reader.ReadBytes(tokenLength);
        //Timestamp
        var timestamp = reader.ReadInt64();
        //Proof
        var proofLength = reader.ReadInt32();
        byte[]? proof = null;
        if (proofLength > 0) {
            proof = reader.ReadBytes(proofLength);
        }
        return new AuthenticationPacket {
            HandshakeId = handshakeId,
            ConnectionToken = token,
            Timestamp = timestamp,
            Proof = proof ?? Array.Empty<byte>()
        };
    }

    /// <summary>
    /// Декодировать AuthenticationResult
    /// </summary>
    public AuthenticationResultPacket DecodeAuthenticationResult(byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);

        var result = new AuthenticationResultPacket {
            Success = reader.ReadBoolean(),
            Message = ReadString(reader),
            VirtualIpAddress = ReadString(reader)
        };

        // DNS серверы
        var dnsCount = reader.ReadInt32();
        result.DnsServers = new string[dnsCount];
        for(int i = 0; i < dnsCount; i++) {
            result.DnsServers[i] = ReadString(reader);
        }

        result.RedirectGateway = reader.ReadBoolean();

        return result;
    }

    /// <summary>
    ///[PacketId:16]
    ///[FragmentIndex:2]
    ///[FragmentCount:2]
    ///[NonceLength:4]
    ///[Nonce]
    ///[TagLength:4]
    ///[Tag]
    ///[EncryptedLength:4]
    ///[EncryptedData]
    /// Декодировать Data пакет
    /// </summary>
    public DataPacket DecodeData (byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);
        var packet = new DataPacket {
            PacketId = new Guid(reader.ReadBytes(16)),
            FragmentIndex = reader.ReadUInt16(),
            FragmentCount = reader.ReadUInt16()
        };
        // Nonce
        var nonceLength = reader.ReadInt32();
        packet.Nonce = reader.ReadBytes(nonceLength);
        // Tag
        var tagLength = reader.ReadInt32();
        packet.Tag = reader.ReadBytes(tagLength);
        // Encrypted data
        var encryptedLength = reader.ReadInt32();
        packet.EncryptedData = reader.ReadBytes(encryptedLength);
        return packet;
    }

    /// <summary>
    /// Декодировать Keepalive
    /// </summary>
    public KeepalivePacket DecodeKeepalive(byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);

        return new KeepalivePacket {
            Timestamp = reader.ReadInt64()
        };
    }

    /// <summary>
    /// Декодировать Ping
    /// </summary>
    public PingPacket DecodePing(byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);

        return new PingPacket {
            Timestamp = reader.ReadInt64(),
            PingId = reader.ReadUInt32()
        };
    }

    /// <summary>
    /// Декодировать Pong
    /// </summary>
    public PongPacket DecodePong(byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);

        return new PongPacket {
            Timestamp = reader.ReadInt64(),
            PingId = reader.ReadUInt32()
        };
    }

    /// <summary>
    /// Декодировать Disconnect
    /// </summary>
    public DisconnectPacket DecodeDisconnect(byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);

        return new DisconnectPacket {
            Reason = ReadString(reader)
        };
    }

    /// <summary>
    /// Декодировать Error
    /// </summary>
    public ErrorPacket DecodeError(byte[] payload) {
        using var ms = new MemoryStream(payload);
        using var reader = new BinaryReader(ms);

        return new ErrorPacket {
            ErrorCode = reader.ReadUInt32(),
            Message = ReadString(reader)
        };
    }

    // Вспомогательные методы
    private string ReadString(BinaryReader reader) {
        var length = reader.ReadInt32();
        if(length == 0)
            return string.Empty;

        var bytes = reader.ReadBytes(length);
        return Encoding.UTF8.GetString(bytes);
    }
}
