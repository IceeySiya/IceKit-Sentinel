namespace IceKitSentinel.Api.Services.PasswordGenerator;

using IceKitSentinel.Api.DTOs.PasswordGenerator;
using System.Security.Cryptography;
public class PasswordGeneratorService
{
      private const string Lowercase =
        "abcdefghijklmnopqrstuvwxyz";

    private const string Uppercase =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private const string Numbers =
        "0123456789";

    private const string SpecialCharacters =
        "!@#$%^&*()-_=+[]{};:,.<>?";
    
    public async Task<PasswordGeneratorResponseDTO> GeneratePasswordAsync(PasswordGeneratorRequestDTO request)
    {
        if(request.Length <= 8)
        {
            throw new ArgumentException("Password length must be greater than 8.");
        }

        if(request.Length > 128)
        {
            throw new ArgumentException("Password length must be less than or equal to 128.");
        }

        var CharacterPool = string.Empty;
        if(request.IncludeLowercase) CharacterPool += Lowercase;
        if(request.IncludeUppercase) CharacterPool += Uppercase;
        if(request.IncludeNumbers) CharacterPool += Numbers;
        if(request.IncludeSpecialCharacters) CharacterPool += SpecialCharacters;
        if(CharacterPool.Length == 0)
        {
            throw new ArgumentException("At least one character type must be selected.");
        }
        var password = GenerateRandomPassword(request.Length, CharacterPool);
        var response = new PasswordGeneratorResponseDTO()
        {
            Password = password,
            Length = request.Length,
            HasLowercase = request.IncludeLowercase,
            HasUppercase = request.IncludeUppercase,
            HasNumbers = request.IncludeNumbers,
            HasSpecialCharacters = request.IncludeSpecialCharacters
        };
        return response;
    }

    private static string GenerateRandomPassword(int length,string characterPool)
    {
        var passwordChars = new char[length];
        for (int i = 0; i<length; i++)
        {
            passwordChars[i] = characterPool[RandomNumberGenerator.GetInt32(characterPool.Length)];
        }
        return new string(passwordChars);
    }
}