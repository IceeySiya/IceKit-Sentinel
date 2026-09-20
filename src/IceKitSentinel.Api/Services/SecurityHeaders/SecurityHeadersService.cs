namespace IceKitSentinel.Api.Services.SecurityHeaders;

using IceKitSentinel.Api.DTOs.SecurityHeaders;

public class SecurityHeadersService
{
    private readonly HttpClient _httpClient;
    private readonly SsrfProtectionService _ssrfProtectionService;

    public SecurityHeadersService(
        HttpClient httpClient,
        SsrfProtectionService ssrfProtectionService)
    {
        _httpClient = httpClient;
        _ssrfProtectionService = ssrfProtectionService;
    }

    public async Task<SecurityHeadersResponseDto>
        AnalyzeAsync(
            SecurityHeadersRequestDto request)
    {
        // Make sure a request object was provided.
        if (request == null)
        {
            throw new ArgumentException(
                "Request cannot be null."
            );
        }

        // Make sure the URL was provided.
        if (string.IsNullOrWhiteSpace(request.Url))
        {
            throw new ArgumentException(
                "URL cannot be empty."
            );
        }

        // Parse the supplied URL.
        if (!Uri.TryCreate(
                request.Url,
                UriKind.Absolute,
                out var uri))
        {
            throw new ArgumentException(
                "Invalid URL."
            );
        }

        // Validate the destination before making
        // a server-side HTTP request.
        await _ssrfProtectionService.ValidateUrlAsync(uri);

        // Make the request only after SSRF validation succeeds.
        using var response =
            await _httpClient.GetAsync(uri);

        // Security headers that IceKit Sentinel checks.
        var headersToCheck = new[]
        {
            "Content-Security-Policy",
            "Strict-Transport-Security",
            "X-Content-Type-Options",
            "X-Frame-Options",
            "Referrer-Policy",
            "Permissions-Policy"
        };

        // Store whether each security header exists.
        var securityHeaders =
            new Dictionary<string, bool>();

        foreach (var header in headersToCheck)
        {
            securityHeaders[header] =
                response.Headers.Contains(header) ||
                response.Content.Headers.Contains(header);
        }

        // Count how many security headers were found.
        var headersPresent =
            securityHeaders.Count(
                header => header.Value
            );

        // Return the analysis result.
        return new SecurityHeadersResponseDto
        {
            Url = request.Url,
            StatusCode = (int)response.StatusCode,
            IsHttps = uri.Scheme == Uri.UriSchemeHttps,
            SecurityHeaders = securityHeaders,
            HeadersPresent = headersPresent,
            HeadersChecked = headersToCheck.Length
        };
    }
}