using BlackProjects.Common.Packets;

using System.Text;

namespace BlackProjects.Common.Protocols;

/// <summary>
/// Кодировщик VPN пакетов в байты
/// </summary>
public class ProtocolEncoder {
    /// <summary>
    /// Закодировать VPN пакет в байты
    /// Формат: [Type(1)][SessionId(16)][SequenceNumber(4)][PayloadLength(4)][Payload(N)]
    /// </summary>
    public byte[] Encode(VpnPacket packet) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        // Заголовок
        writer.Write((byte)packet.Type);
        writer.Write(packet.SessionId.ToByteArray());
        writer.Write(packet.SequenceNumber);
        writer.Write(packet.Payload.Length);
        // Полезная нагрузка
        writer.Write(packet.Payload);
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать ConnectRequest
    /// </summary>
    public byte[] EncodeConnectRequest (ConnectRequestPacket request) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        //Записываем длину и сам публичный ключ
        writer.Write(request.ClientPublicKey.Length);
        writer.Write(request.ClientPublicKey);
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать ConnectResponse
    /// </summary>
    public byte[] EncodeConnectResponse (ConnectResponsePacket response) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        // HandshakeId (16 байт)
        Span<byte> guidBytes = stackalloc byte[16];
        response.HandshakeId.TryWriteBytes(guidBytes);
        writer.Write(guidBytes);
        // Accepted (bool? — кодируем как byte: 0=false, 1=true, 2=null)
        byte acceptedStatus = response.Accepted switch {
            false => 0,
            true => 1,
            _ => 2
        };
        writer.Write(acceptedStatus);
        //Message
        WriteString(writer, response.Message);
        //ServerPublicKey (Длина + Ключ)
        writer.Write(response.ServerPublicKey.Length);
        writer.Write(response.ServerPublicKey);
        // Challenge
        writer.Write(response.Challenge?.Length ?? 0);
        if (response.Challenge != null && response.Challenge.Length > 0) {
            writer.Write(response.Challenge);
        }
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать Authentication
    /// </summary>
    public byte[] EncodeAuthentication(AuthenticationPacket auth) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        Span<byte> guidBytes = stackalloc byte[16];
        auth.HandshakeId.TryWriteBytes(guidBytes);
        writer.Write(guidBytes);
        writer.Write(auth.ConnectionToken.Length);
        writer.Write(auth.ConnectionToken);
        //Timestamp (8 байт - long)
        writer.Write(auth.Timestamp);
        //Proof (Длина + Proof)
        writer.Write(auth.Proof?.Length ?? 0);
        if (auth.Proof != null && auth.Proof.Length > 0) {
            writer.Write(auth.Proof);
        }
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать AuthenticationResult
    /// </summary>
    public byte[] EncodeAuthenticationResult(AuthenticationResultPacket result) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(result.Success);
        WriteString(writer, result.Message);
        WriteString(writer, result.VirtualIpAddress ?? string.Empty);
        // DNS серверы
        writer.Write(result.DnsServers.Length);
        foreach(var dns in result.DnsServers) {
            WriteString(writer, dns);
        }
        writer.Write(result.RedirectGateway);
        return ms.ToArray();
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
    /// Закодировать Data пакет
    /// </summary>
    public byte[] EncodeData (DataPacket data) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        // PacketId (16 bytes)
        writer.Write(data.PacketId.ToByteArray());
        // Fragment info
        writer.Write(data.FragmentIndex);
        writer.Write(data.FragmentCount);
        // Nonce
        writer.Write(data.Nonce.Length);
        writer.Write(data.Nonce);
        // Tag
        writer.Write(data.Tag.Length);
        writer.Write(data.Tag);
        // Encrypted payload
        writer.Write(data.EncryptedData.Length);
        writer.Write(data.EncryptedData);
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать Keepalive
    /// </summary>
    public byte[] EncodeKeepalive(KeepalivePacket keepalive) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(keepalive.Timestamp);
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать Ping
    /// </summary>
    public byte[] EncodePing(PingPacket ping) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(ping.Timestamp);
        writer.Write(ping.PingId);
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать Pong
    /// </summary>
    public byte[] EncodePong(PongPacket pong) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(pong.Timestamp);
        writer.Write(pong.PingId);
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать Disconnect
    /// </summary>
    public byte[] EncodeDisconnect(DisconnectPacket disconnect) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        WriteString(writer, disconnect.Reason);
        return ms.ToArray();
    }

    /// <summary>
    /// Закодировать Error
    /// </summary>
    public byte[] EncodeError(ErrorPacket error) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(error.ErrorCode);
        WriteString(writer, error.Message);
        return ms.ToArray();
    }

    // Вспомогательные методы
    private void WriteString(BinaryWriter writer, string value) {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }
}
