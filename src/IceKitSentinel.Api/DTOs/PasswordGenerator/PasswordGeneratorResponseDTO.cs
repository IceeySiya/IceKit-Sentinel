namespace IceKitSentinel.Api.DTOs.PasswordGenerator;

public class PasswordGeneratorResponseDTO
{
    public string Password {get; set;} = string.Empty;
    public int Length {get; set;} = 0;
    public bool HasLowercase {get; set;} = false;
    public bool HasUppercase {get; set;} = false;
    public bool HasNumbers {get; set;} = false;
    public bool HasSpecialCharacters {get; set;} = false;
}