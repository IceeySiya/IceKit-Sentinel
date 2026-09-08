namespace IceKitSentinel.Api.DTOs.Hasher;

public class HashResponseDTO
{
    public string Algorithm {get; set;}=string.Empty;
    public string Hash {get; set;}=string.Empty;
    public int InputLength {get; set;}
}