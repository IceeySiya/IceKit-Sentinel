namespace IceKitSentinel.Api.DTOs.FileIntegrity;

public class FileIntegrityResponseDto
{
    public int Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string FileHash { get; set; } = string.Empty;

    public string Algorithm { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}