using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IceKitSentinel.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace IceKitSentinel.Api.Services;

// Generates JWT tokens after a user successfully logs in.
public class JwtService
{
// Stores the application configuration.
private readonly IConfiguration _configuration;


// Receives configuration through dependency injection.
public JwtService(IConfiguration configuration)
{
    _configuration = configuration;
}

// Creates and returns a signed JWT for the authenticated user.
public string GenerateToken(User user)
{
    // Reads the JWT settings from appsettings.Development.json.
    var key = _configuration["Jwt:Key"]
        ?? throw new InvalidOperationException(
            "JWT key is not configured.");

    var issuer = _configuration["Jwt:Issuer"]
        ?? throw new InvalidOperationException(
            "JWT issuer is not configured.");

    var audience = _configuration["Jwt:Audience"]
        ?? throw new InvalidOperationException(
            "JWT audience is not configured.");

    var expiryMinutes = _configuration.GetValue<int>(
        "Jwt:ExpiryMinutes");

    // Creates claims containing safe user identity information.
    // Never add passwords or password hashes to JWT claims.
    var claims = new[]
    {
        new Claim(
            JwtRegisteredClaimNames.Sub,
            user.Id.ToString()),

        new Claim(
            JwtRegisteredClaimNames.Email,
            user.Email),

        new Claim(
            JwtRegisteredClaimNames.UniqueName,
            user.UserName)
    };

    // Creates a security key from the JWT secret.
    var securityKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(key));

    // Uses the secret key to sign the token.
    var credentials = new SigningCredentials(
        securityKey,
        SecurityAlgorithms.HmacSha256);

    // Creates the JWT token.
    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(
            expiryMinutes),
        signingCredentials: credentials);

    // Converts the JWT object into a string
    // that can be returned to the client.
    return new JwtSecurityTokenHandler()
        .WriteToken(token);
}


}
