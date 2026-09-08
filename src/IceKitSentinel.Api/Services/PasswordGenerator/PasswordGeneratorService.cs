using IceKitSentinel.Api.DTOs.PasswordGenerator;
using System.Security.Cryptography;

namespace IceKitSentinel.Api.Services.PasswordGenerator;

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
        if(request.Length < 12)
        {
            throw new ArgumentException("Password length must be greater than or equal to 12.");
        }

        if(request.Length > 128)
        {
            throw new ArgumentException("Password length must be less than or equal to 128.");
        }

        var CharacterPool = new  List<string>();
        if(request.IncludeLowercase) CharacterPool.Add(Lowercase);
        if(request.IncludeUppercase) CharacterPool.Add(Uppercase);
        if(request.IncludeNumbers) CharacterPool.Add(Numbers);
        if(request.IncludeSpecialCharacters) CharacterPool.Add(SpecialCharacters);
        if(CharacterPool.Count == 0)
        {
            throw new ArgumentException("At least one character type must be selected.");
        }
        if(request.Length < CharacterPool.Count)
        {
            throw new ArgumentException("Password length must be at least as long as the selected character types.");
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

    private static string GenerateRandomPassword(int length,List<string> characterPool)
    {
        var passwordChars ="";
        for(int i = 0; i <characterPool.Count; i++)
        {
            passwordChars += characterPool[i][RandomNumberGenerator.GetInt32(characterPool[i].Length)];
        }
        if(length > passwordChars.Length)
        {
            var allChars= string.Join("",characterPool);
            while(passwordChars.Length < length)
            {
                passwordChars += allChars[RandomNumberGenerator.GetInt32(allChars.Length)];
            }
        }
        return new string(passwordChars);
    }
}