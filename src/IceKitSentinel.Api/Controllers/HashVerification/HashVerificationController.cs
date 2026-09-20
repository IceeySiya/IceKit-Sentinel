namespace IceKitSentinel.Api.Controllers.HashVerification;

using IceKitSentinel.Api.DTOs.HashVerification;
using IceKitSentinel.Api.Services.HashVerification;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HashVerificationController : ControllerBase
{
    private readonly HashVerificationService _hashVerificationService;

    public HashVerificationController()
    {
        _hashVerificationService = new HashVerificationService();
    }

    [HttpPost("verify")]
    public IActionResult VerifyHash(
        HashVerificationRequestDto request)
    {
        try
        {
            var result =
                _hashVerificationService.VerifyHash(request);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}