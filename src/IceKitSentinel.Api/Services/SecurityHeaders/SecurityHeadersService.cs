namespace IceKitSentinel.Api.Services.SecurityHeaders;

using IceKitSentinel.Api.DTOs.SecurityHeaders;

public class SecurityHeadersService
{
    private const int MaxRedirects = 5;

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

        // Validate the initial destination.
        await _ssrfProtectionService.ValidateUrlAsync(uri);

        var currentUri = uri;
        var redirectCount = 0;

        HttpResponseMessage response;

        while (true)
        {
            // Make the request only after the destination
            // has passed SSRF validation.
            response =
                await _httpClient.GetAsync(currentUri);

            // If this isn't a redirect, we're finished.
            if (!IsRedirect(response))
            {
                break;
            }

            redirectCount++;

            // Prevent endless redirect chains.
            if (redirectCount > MaxRedirects)
            {
                response.Dispose();

                throw new InvalidOperationException(
                    "The target URL redirected too many times."
                );
            }

            // A redirect should contain a Location header.
            if (response.Headers.Location == null)
            {
                response.Dispose();

                throw new InvalidOperationException(
                    "The target server returned a redirect without a destination."
                );
            }

            // Resolve relative redirects against the current URL.
            var redirectUri =
                response.Headers.Location.IsAbsoluteUri
                    ? response.Headers.Location
                    : new Uri(
                        currentUri,
                        response.Headers.Location
                    );

            response.Dispose();

            // Validate the REDIRECT destination before following it.
            await _ssrfProtectionService
                .ValidateUrlAsync(redirectUri);

            currentUri = redirectUri;
        }

        using (response)
        {
            var headersToCheck = new[]
            {
                "Content-Security-Policy",
                "Strict-Transport-Security",
                "X-Content-Type-Options",
                "X-Frame-Options",
                "Referrer-Policy",
                "Permissions-Policy"
            };

            var securityHeaders =
                new Dictionary<string, bool>();

            foreach (var header in headersToCheck)
            {
                securityHeaders[header] =
                    response.Headers.Contains(header) ||
                    response.Content.Headers.Contains(header);
            }

            var headersPresent =
                securityHeaders.Count(
                    header => header.Value
                );

            return new SecurityHeadersResponseDto
            {
                // Return the final URL that was actually analyzed.
                Url = currentUri.ToString(),

                StatusCode = (int)response.StatusCode,

                IsHttps =
                    currentUri.Scheme ==
                    Uri.UriSchemeHttps,

                SecurityHeaders = securityHeaders,

                HeadersPresent = headersPresent,

                HeadersChecked = headersToCheck.Length
            };
        }
    }

    private static bool IsRedirect(
        HttpResponseMessage response)
    {
        return (int)response.StatusCode >= 300 &&
               (int)response.StatusCode <= 399;
    }
}