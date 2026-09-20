namespace IceKitSentinel.Api.Controllers.SecurityHeaders;

using IceKitSentinel.Api.DTOs.SecurityHeaders;
using IceKitSentinel.Api.Services.SecurityHeaders;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SecurityHeadersController : ControllerBase
{
    private readonly SecurityHeadersService _securityHeadersService;

    public SecurityHeadersController(
        SecurityHeadersService securityHeadersService)
    {
        _securityHeadersService = securityHeadersService;
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze(
        SecurityHeadersRequestDto request)
    {
        try
        {
            var result =
                await _securityHeadersService.AnalyzeAsync(request);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new
            {
                message =
                    $"Unable to reach the target URL: {ex.Message}"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message =
                        "An unexpected error occurred while analyzing the security headers.",
                    details = ex.Message
                }
            );
        }
    }
}