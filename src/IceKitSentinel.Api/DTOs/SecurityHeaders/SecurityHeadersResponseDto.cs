namespace IceKitSentinel.Api.DTOs.SecurityHeaders;

public class SecurityHeadersResponseDto
{
    public string Url { get; set; } = string.Empty;

    public int StatusCode { get; set; }

    public bool IsHttps { get; set; }

    public Dictionary<string, bool> SecurityHeaders { get; set; }
        = new();

    public int HeadersPresent { get; set; }

    public int HeadersChecked { get; set; }
}