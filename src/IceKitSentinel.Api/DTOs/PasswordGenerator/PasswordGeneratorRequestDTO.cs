namespace IceKitSentinel.Api.DTOs.PasswordGenerator;

public class PasswordGeneratorRequestDTO
{
    public int Length {get; set;} = 12;
    public bool IncludeLowercase {get; set;} = true;
    public bool IncludeUppercase {get; set;} = true;
    public bool IncludeNumbers {get; set;} = true;
    public bool IncludeSpecialCharacters {get; set;} = true;
}