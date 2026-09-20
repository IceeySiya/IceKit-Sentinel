namespace IceKitSentinel.Api.Models;

public class FileIntegrityRecord
{
    public int Id { get; set; }

    // Name of the file being monitored.
    public string FileName { get; set; } = string.Empty;

    // Path of the file on the system.
    public string FilePath { get; set; } = string.Empty;

    // Cryptographic hash of the known-good file.
    public string FileHash { get; set; } = string.Empty;

    // Hash algorithm used to generate the hash.
    public string Algorithm { get; set; } = "SHA256";

    // Size of the file in bytes.
    public long FileSize { get; set; }

    // When the integrity record was created.
    public DateTime CreatedAt { get; set; }

    // When the record was last updated.
    public DateTime UpdatedAt { get; set; }
}