namespace IceKitSentinel.Api.DTOs.FileIntegrity;

public class FileIntegrityCheckResponseDto
{
    public int FileIntegrityRecordId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string Algorithm { get; set; } = string.Empty;

    public string ExpectedHash { get; set; } = string.Empty;

    public string GeneratedHash { get; set; } = string.Empty;

    public bool IsMatch { get; set; }

    public string Message { get; set; } = string.Empty;
}