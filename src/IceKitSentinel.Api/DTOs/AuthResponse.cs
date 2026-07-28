using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace IceKitSentinel.Api.DTOs;

public class AuthResponse
{
    public int Id {get;set;}
    public string UserName {get;set;}=string.Empty;
    public string Email {get;set;}=string.Empty;
    public string Message {get;set;}=string.Empty;
    
    // JWT token returned after successful login.
    // The client sends this token with protected requests.
    public string? Token { get; set; }
}