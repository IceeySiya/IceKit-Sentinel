using IceKitSentinel.Api.Data;
using IceKitSentinel.Api.DTOs;
using IceKitSentinel.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IceKitSentinel.Api.Services;

namespace IceKitSentinel.Api.Controllers;

// Marks this class as an API controller.
[ApiController]

// Sets the base route to: /api/auth
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
// Provides access to the PostgreSQL database through EF Core.
private readonly IceKitDbContext _context;
// Generates JWT tokens after successful login.
private readonly JwtService _jwtService;


// Hashes passwords during registration
// and verifies passwords during login.
private readonly PasswordHasher<User> _passwordHasher;

// Uses dependency injection to receive the database context
// and password hasher.
public AuthController(
    IceKitDbContext context,
    PasswordHasher<User> passwordHasher,
    JwtService jwtService)
{
    _context = context;
    _passwordHasher = passwordHasher;
    _jwtService=jwtService;
}

// Handles POST requests sent to:
// /api/auth/register
[HttpPost("register")]
public async Task<ActionResult<AuthResponse>> Register(
    RegisterRequest request)
{
    // Checks whether a user already exists with this email.
    var emailExists = await _context.Users
        .AnyAsync(user => user.Email == request.Email);

    // Returns HTTP 409 Conflict if the email is already registered.
    if (emailExists)
    {
        return Conflict(new
        {
            message = "Email Already Exists."
        });
    }

    // Creates a new User object from the registration request.
    var user = new User
    {
        UserName = request.UserName,
        Email = request.Email
    };

    // Hashes the password before saving it.
    // The plain-text password is never stored in PostgreSQL.
    user.PasswordHash = _passwordHasher.HashPassword(
        user,
        request.Password);

    // Adds the user to EF Core's change tracker.
    _context.Users.Add(user);

    // Saves the new user to the PostgreSQL database.
    await _context.SaveChangesAsync();

    // Returns HTTP 201 Created with safe user information.
    // PasswordHash is not included in the response.
    return Created(
        $"/api/auth/{user.Id}",
        new AuthResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Message = "User Registered Successfully."
        });
}

// Handles POST requests sent to:
// /api/auth/login
[HttpPost("login")]
public async Task<ActionResult<AuthResponse>> Login(
    LoginRequest request)
{
    // Searches for a user with the supplied email address.
    var user = await _context.Users
        .FirstOrDefaultAsync(
            user => user.Email == request.Email);

    // Returns HTTP 401 if the email is not registered.
    // A generic message prevents revealing which detail was incorrect.
    if (user is null)
    {
        return Unauthorized(new
        {
            message = "Invalid Email or Password"
        });
    }

    // Compares the entered password with the stored password hash.
    var verificationResult =
        _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

    // Returns HTTP 401 if password verification fails.
    if (verificationResult ==
        PasswordVerificationResult.Failed)
    {
        return Unauthorized(new
        {
            message = "Invalid Email or Password"
        });
    }

    // Returns HTTP 200 OK when login is successful.
    // The password hash is never exposed.
    var token = _jwtService.GenerateToken(user);
    return Ok(new AuthResponse
    {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        Message = "Login Successful",
        Token=token
    });
}


}
