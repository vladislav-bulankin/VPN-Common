namespace BlackProjects.Common.Entities; 
public class TunConfiguration {
    /// <summary>
    /// Имя интерфейса (например "tun0")
    /// </summary>
    public string Name { get; set; } = "vpntun0";

    /// <summary>
    /// IP адрес сервера в VPN сети (например "10.8.0.1")
    /// </summary>
    public string ServerIpAddress { get; set; } = "10.8.0.1";

    /// <summary>
    /// Маска подсети (например "255.255.255.0")
    /// </summary>
    public string SubnetMask { get; set; } = "255.255.255.0";

    /// <summary>
    /// Подсеть в CIDR нотации (например "10.8.0.0/24")
    /// </summary>
    public string Subnet { get; set; } = "10.8.0.0/24";

    /// <summary>
    /// MTU
    /// </summary>
    public int Mtu { get; set; } = 1500;

    /// <summary>
    /// Включить NAT (Network Address Translation)
    /// </summary>
    public bool EnableNat { get; set; } = true;

    /// <summary>
    /// Включить IP forwarding
    /// </summary>
    public bool EnableIpForwarding { get; set; } = true;
}
