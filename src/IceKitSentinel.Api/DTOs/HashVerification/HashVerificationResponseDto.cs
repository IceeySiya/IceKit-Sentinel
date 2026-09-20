namespace IceKitSentinel.Api.DTOs.HashVerification;

public class HashVerificationResponseDto
{
    // The algorithm used during verification.
    public string Algorithm { get; set; } = string.Empty;

    // The hash generated from the supplied input.
    public string GeneratedHash { get; set; } = string.Empty;

    // Whether the generated hash matches the expected hash.
    public bool IsMatch { get; set; }
}