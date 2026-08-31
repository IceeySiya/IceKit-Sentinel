namespace IceKitSentinel.Api.DTOs;

public class PasswordAnalysisResponse
{
    public int Score { get; set; }

    public string Strength { get; set; } = string.Empty;

    public int Length { get; set; }

    public bool HasLowercase { get; set; }

    public bool HasUppercase { get; set; }

    public bool HasNumbers { get; set; }

    public bool HasSpecialCharacters { get; set; }

    public List<string> Recommendations { get; set; } = new();
}