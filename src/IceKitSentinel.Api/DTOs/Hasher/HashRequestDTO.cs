namespace IceKitSentinel.Api.DTOs.Hasher;

public class HashRequestDTO
{
    public string input {get; set;}=string.Empty;
    public string Algorithm {get; set;}="SHA256";
}