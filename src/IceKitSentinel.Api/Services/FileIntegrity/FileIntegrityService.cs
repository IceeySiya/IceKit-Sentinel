namespace IceKitSentinel.Api.Services.FileIntegrity;

using IceKitSentinel.Api.Data;
using IceKitSentinel.Api.DTOs.FileIntegrity;
using IceKitSentinel.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

public class FileIntegrityService
{
    private readonly IceKitDbContext _dbContext;

    public FileIntegrityService(IceKitDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // ---------------------------------------------------------
    // Generate a cryptographic hash for an uploaded file.
    // ---------------------------------------------------------
    public async Task<string> GenerateFileHashAsync(
        IFormFile file,
        string algorithm)
    {
        // Make sure a file was actually provided.
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException(
                "File cannot be empty."
            );
        }

        // Make sure an algorithm was provided.
        if (string.IsNullOrWhiteSpace(algorithm))
        {
            throw new ArgumentException(
                "Algorithm must be specified."
            );
        }

        // Open the uploaded file as a stream.
        await using var stream = file.OpenReadStream();

        byte[] hashBytes;

        // Generate the hash using the selected algorithm.
        switch (algorithm.ToUpperInvariant())
        {
            case "SHA256":
                hashBytes = await SHA256.HashDataAsync(stream);
                break;

            case "SHA512":
                hashBytes = await SHA512.HashDataAsync(stream);
                break;

            default:
                throw new ArgumentException(
                    "Unsupported algorithm. Please use SHA256 or SHA512."
                );
        }

        // Convert the binary hash into a readable
        // hexadecimal string.
        return Convert.ToHexString(hashBytes)
            .ToLowerInvariant();
    }


    // ---------------------------------------------------------
    // Register a file as a known-good baseline.
    // ---------------------------------------------------------
    public async Task<FileIntegrityRecord> RegisterFileAsync(
        FileIntegrityRegisterRequestDto request)
    {
        // Make sure the request exists.
        if (request == null)
        {
            throw new ArgumentException(
                "Request cannot be null."
            );
        }

        // Validate that a file was provided.
        if (request.File == null || request.File.Length == 0)
        {
            throw new ArgumentException(
                "File cannot be empty."
            );
        }

        // Validate the algorithm.
        if (string.IsNullOrWhiteSpace(request.Algorithm))
        {
            throw new ArgumentException(
                "Algorithm must be specified."
            );
        }

        // Normalize the algorithm.
        var algorithm =
            request.Algorithm.ToUpperInvariant();

        // Generate the hash of the uploaded file.
        var hash = await GenerateFileHashAsync(
            request.File,
            algorithm
        );

        // Check whether this exact file fingerprint
        // has already been registered.
        var existingRecord =
            await _dbContext.FileIntegrityRecords
                .FirstOrDefaultAsync(record =>
                    record.FileName ==
                        request.File.FileName &&
                    record.FileHash ==
                        hash &&
                    record.Algorithm ==
                        algorithm
                );

        // Prevent duplicate registrations.
        if (existingRecord != null)
        {
            throw new InvalidOperationException(
                "This file has already been registered."
            );
        }

        // Create the database record.
        var record = new FileIntegrityRecord
        {
            FileName = request.File.FileName,

            // A browser upload does not provide the user's
            // actual local filesystem path.
            FilePath = request.File.FileName,

            FileHash = hash,

            Algorithm = algorithm,

            FileSize = request.File.Length,

            CreatedAt = DateTime.UtcNow,

            UpdatedAt = DateTime.UtcNow
        };

        // Add the record to the database.
        _dbContext.FileIntegrityRecords.Add(record);

        // Save the record.
        await _dbContext.SaveChangesAsync();

        return record;
    }


    // ---------------------------------------------------------
    // Get all registered file fingerprints.
    // ---------------------------------------------------------
    public async Task<List<FileIntegritySummaryDto>>
        GetAllFilesAsync()
    {
        return await _dbContext.FileIntegrityRecords
            .AsNoTracking()
            .OrderByDescending(record => record.CreatedAt)
            .Select(record => new FileIntegritySummaryDto
            {
                Id = record.Id,

                FileName = record.FileName,

                FileHash = record.FileHash,

                Algorithm = record.Algorithm,

                FileSize = record.FileSize,

                CreatedAt = record.CreatedAt
            })
            .ToListAsync();
    }


    // ---------------------------------------------------------
    // Get one registered file fingerprint by ID.
    // ---------------------------------------------------------
    public async Task<FileIntegrityResponseDto?>
        GetFileByIdAsync(int id)
    {
        return await _dbContext.FileIntegrityRecords
            .AsNoTracking()
            .Where(record => record.Id == id)
            .Select(record => new FileIntegrityResponseDto
            {
                Id = record.Id,

                FileName = record.FileName,

                FilePath = record.FilePath,

                FileHash = record.FileHash,

                Algorithm = record.Algorithm,

                FileSize = record.FileSize,

                CreatedAt = record.CreatedAt,

                UpdatedAt = record.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }


    // ---------------------------------------------------------
    // Check an uploaded file against a stored fingerprint.
    // ---------------------------------------------------------
    public async Task<FileIntegrityCheckResponseDto>
        CheckFileIntegrityAsync(
            FileIntegrityCheckRequestDto request)
    {
        // Make sure the request exists.
        if (request == null)
        {
            throw new ArgumentException(
                "Request cannot be null."
            );
        }

        // Validate the uploaded file.
        if (request.File == null ||
            request.File.Length == 0)
        {
            throw new ArgumentException(
                "File cannot be empty."
            );
        }

        // Validate the database record ID.
        if (request.FileIntegrityRecordId <= 0)
        {
            throw new ArgumentException(
                "A valid file integrity record ID is required."
            );
        }

        // Find the registered baseline in the database.
        var baseline =
            await _dbContext.FileIntegrityRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(record =>
                    record.Id ==
                    request.FileIntegrityRecordId
                );

        // The requested baseline does not exist.
        if (baseline == null)
        {
            throw new KeyNotFoundException(
                "The selected file fingerprint was not found."
            );
        }

        // Generate a hash for the uploaded file
        // using the same algorithm as the baseline.
        var generatedHash =
            await GenerateFileHashAsync(
                request.File,
                baseline.Algorithm
            );

        // Compare the newly generated hash against
        // the hash stored in the database.
        var isMatch =
            string.Equals(
                generatedHash,
                baseline.FileHash,
                StringComparison.OrdinalIgnoreCase
            );

        // Create the response.
        return new FileIntegrityCheckResponseDto
        {
            FileIntegrityRecordId = baseline.Id,

            FileName = request.File.FileName,

            FileSize = request.File.Length,

            Algorithm = baseline.Algorithm,

            ExpectedHash = baseline.FileHash,

            GeneratedHash = generatedHash,

            IsMatch = isMatch,

            Message = isMatch
                ? "File integrity verified successfully."
                : "File integrity check failed. The file may have been modified."
        };
    }
}