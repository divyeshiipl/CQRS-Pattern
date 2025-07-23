namespace Domain.Common;

public static class NetworkUtils
{
    public static string GetServerIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
        return ipAddress ?? "Unknown";
    }
}
