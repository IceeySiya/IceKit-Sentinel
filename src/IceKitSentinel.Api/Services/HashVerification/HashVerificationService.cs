namespace IceKitSentinel.Api.Services.HashVerification;

using IceKitSentinel.Api.DTOs.HashVerification;
using IceKitSentinel.Api.DTOs.Hasher;
using IceKitSentinel.Api.Services.Hasher;

public class HashVerificationService
{
    private readonly HashService _hashService;

    public HashVerificationService()
    {
        _hashService = new HashService();
    }

    public HashVerificationResponseDto VerifyHash(
        HashVerificationRequestDto request)
    {
        // Validate the input.
        if (string.IsNullOrEmpty(request.Input))
        {
            throw new ArgumentException(
                "Input cannot be empty."
            );
        }

        // Validate the expected hash.
        if (string.IsNullOrWhiteSpace(request.ExpectedHash))
        {
            throw new ArgumentException(
                "Expected hash cannot be empty."
            );
        }

        // Create a request for the existing HashService.
        var hashRequest = new HashRequestDTO
        {
            input = request.Input,
            Algorithm = request.Algorithm
        };

        // Generate the hash using the existing HashService.
        var hashResponse = _hashService.GenerateHash(hashRequest);

        // Compare the generated hash with the expected hash.
        bool isMatch = string.Equals(
            hashResponse.Hash,
            request.ExpectedHash.Trim(),
            StringComparison.OrdinalIgnoreCase
        );

        // Return the verification result.
        return new HashVerificationResponseDto
        {
            Algorithm = hashResponse.Algorithm,
            GeneratedHash = hashResponse.Hash,
            IsMatch = isMatch
        };
    }
}