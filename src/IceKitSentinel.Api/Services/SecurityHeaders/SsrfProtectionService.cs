namespace IceKitSentinel.Api.Services.SecurityHeaders;

using System.Net;
using System.Net.Sockets;

public class SsrfProtectionService
{
    public async Task ValidateUrlAsync(Uri uri)
    {
        if (uri == null)
        {
            throw new ArgumentException("URL cannot be null.");
        }

        if (uri.Scheme != Uri.UriSchemeHttp &&
            uri.Scheme != Uri.UriSchemeHttps)
        {
            throw new ArgumentException(
                "Only HTTP and HTTPS URLs are supported."
            );
        }

        if (string.IsNullOrWhiteSpace(uri.Host))
        {
            throw new ArgumentException(
                "URL must contain a valid host."
            );
        }

        IPAddress[] addresses;

        if (IPAddress.TryParse(uri.Host, out var parsedAddress))
        {
            addresses = new[] { parsedAddress };
        }
        else
        {
            try
            {
                addresses =
                    await Dns.GetHostAddressesAsync(uri.Host);
            }
            catch (SocketException)
            {
                throw new ArgumentException(
                    "The target hostname could not be resolved."
                );
            }
        }

        foreach (var address in addresses)
        {
            if (IsBlockedAddress(address))
            {
                throw new ArgumentException(
                    "The target URL resolves to a restricted network address."
                );
            }
        }
    }

    private static bool IsBlockedAddress(IPAddress address)
    {
        if (IPAddress.IsLoopback(address))
        {
            return true;
        }

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            return IsPrivateIpv4(address);
        }

        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return IsRestrictedIpv6(address);
        }

        return true;
    }

    private static bool IsPrivateIpv4(IPAddress address)
    {
        var bytes = address.GetAddressBytes();

        // 10.0.0.0/8
        if (bytes[0] == 10)
        {
            return true;
        }

        // 172.16.0.0/12
        if (bytes[0] == 172 &&
            bytes[1] >= 16 &&
            bytes[1] <= 31)
        {
            return true;
        }

        // 192.168.0.0/16
        if (bytes[0] == 192 &&
            bytes[1] == 168)
        {
            return true;
        }

        // 169.254.0.0/16 - link-local
        if (bytes[0] == 169 &&
            bytes[1] == 254)
        {
            return true;
        }

        return false;
    }

    private static bool IsRestrictedIpv6(IPAddress address)
    {
        if (address.Equals(IPAddress.IPv6Loopback))
        {
            return true;
        }

        if (address.IsIPv6LinkLocal)
        {
            return true;
        }

        if (address.IsIPv6SiteLocal)
        {
            return true;
        }

        return false;
    }
}