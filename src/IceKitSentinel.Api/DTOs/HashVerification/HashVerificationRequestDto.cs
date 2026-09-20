namespace IceKitSentinel.Api.DTOs.HashVerification;

public class HashVerificationRequestDto
{
    // The original text we want to verify.
    public string Input { get; set; } = string.Empty;

    // The hash algorithm to use.
    // Supported values: SHA256 and SHA512.
    public string Algorithm { get; set; } = "SHA256";

    // The hash we want to compare against.
    public string ExpectedHash { get; set; } = string.Empty;
}