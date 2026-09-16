namespace IceKitSentinel.Api.Controllers.PasswordGenerator;

using IceKitSentinel.Api.DTOs.PasswordGenerator;
using IceKitSentinel.Api.Services.PasswordGenerator;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PasswordGeneratorController : ControllerBase
{
    private readonly PasswordGeneratorService _passwordGeneratorService;

    public PasswordGeneratorController()
    {
        _passwordGeneratorService = new PasswordGeneratorService();
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GeneratePassword(
        PasswordGeneratorRequestDTO requestDTO)
    {
        try
        {
            var passwordResponse =
                await _passwordGeneratorService.GeneratePasswordAsync(requestDTO);

            return Ok(passwordResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}