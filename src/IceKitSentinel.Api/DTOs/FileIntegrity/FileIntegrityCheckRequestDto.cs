namespace IceKitSentinel.Api.DTOs.FileIntegrity;

public class FileIntegrityCheckRequestDto
{
    public IFormFile File { get; set; } = null!;

    public int FileIntegrityRecordId { get; set; }
}