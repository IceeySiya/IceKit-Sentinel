using IceKitSentinel.Api.DTOs;
using IceKitSentinel.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace IceKitSentinel.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PasswordAnalysisController : ControllerBase
{
    private readonly PasswordSecurityService _passwordSecurityService;

    public PasswordAnalysisController()
    {
        _passwordSecurityService = new PasswordSecurityService();
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzePassword(PasswordAnalysisRequest request)

    {
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new
            {
                message = "Password cannot be empty."
            });
        }
        var analysisResult = _passwordSecurityService.Analyze(request);
        return Ok(analysisResult);
    }

}