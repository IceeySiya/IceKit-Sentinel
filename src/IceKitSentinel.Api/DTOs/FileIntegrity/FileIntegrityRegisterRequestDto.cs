namespace IceKitSentinel.Api.DTOs.FileIntegrity;

public class FileIntegrityRegisterRequestDto
{
    // The file we want to monitor.
    public IFormFile File { get; set; } = null!;

    // The hashing algorithm used to create the baseline.
    public string Algorithm { get; set; } = "SHA256";
}