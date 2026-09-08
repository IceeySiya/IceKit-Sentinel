using IceKitSentinel.Api.DTOs.Hasher;
using System.Security.Cryptography;
using System.Text;

namespace IceKitSentinel.Api.Services.Hasher;

public class HashService
{
    public HashResponseDTO GenerateHash(HashRequestDTO request)
    {
        //validating the request
        if (string.IsNullOrEmpty(request.input))
        {
            throw new ArgumentException("Input must no be null or empty");
        }

        if (string.IsNullOrWhiteSpace(request.Algorithm))
        {
            throw new ArgumentException("Algorithm must be specified");
        }


        //creating the hash only if the Algorithm specified is supported for now SHA256 and SHA512 are supported
        var inputBytes = Encoding.UTF8.GetBytes(request.input); //cryptographic hash functions operate on bytes.

        Byte[] bytes;

        switch (request.Algorithm.ToUpperInvariant())
        {
            case "SHA256":
                bytes= SHA256.HashData(inputBytes);
                break;
            
            case "SHA512":
                bytes = SHA512.HashData(inputBytes);
                break;
            default:
                throw new ArgumentException("Unsupported Algorithm. Please use SHA256 or SHA512.");
        }


        //returning the hash response
        var hash = Convert.ToHexString(bytes).ToLowerInvariant();//converts the binary hash into a readable format.

        return new HashResponseDTO
        {
            Algorithm=request.Algorithm.ToUpperInvariant(),
            Hash= hash,
            InputLength=request.input.Length

        };
    }
}

