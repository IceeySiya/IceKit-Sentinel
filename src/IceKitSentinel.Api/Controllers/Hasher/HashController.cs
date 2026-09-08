using IceKitSentinel.Api.Services.Hasher;
using IceKitSentinel.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using IceKitSentinel.Api.DTOs.Hasher;

namespace IceKitSentinel.Api.Controllers.Hasher;

[ApiController]
[Route("api/[controller]")]
public class HashController : ControllerBase
{
    private readonly HashService _hashService;

    public HashController()
    {
        _hashService=new HashService();
    }
    
    //Endpoint is POST /api/Hash/generate
    [HttpPost("generate")]
    public IActionResult generateHash(HashRequestDTO request)
    {
        try
        {
            var results =_hashService.GenerateHash(request);
            return Ok(results);
        }catch(ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}

