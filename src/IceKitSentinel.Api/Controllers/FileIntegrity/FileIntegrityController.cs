namespace IceKitSentinel.Api.Controllers.FileIntegrity;

using IceKitSentinel.Api.DTOs.FileIntegrity;
using IceKitSentinel.Api.Services.FileIntegrity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FileIntegrityController : ControllerBase
{
    private readonly FileIntegrityService _fileIntegrityService;

    public FileIntegrityController(
        FileIntegrityService fileIntegrityService)
    {
        _fileIntegrityService = fileIntegrityService;
    }


    // ---------------------------------------------------------
    // Register a file as a known-good baseline.
    // ---------------------------------------------------------
    [HttpPost("register")]
    public async Task<IActionResult> RegisterFile(
        [FromForm] FileIntegrityRegisterRequestDto request)
    {
        try
        {
            var result =
                await _fileIntegrityService.RegisterFileAsync(
                    request
                );

            return Ok(new
            {
                message = "File registered successfully.",
                id = result.Id,
                fileName = result.FileName,
                fileSize = result.FileSize,
                algorithm = result.Algorithm,
                fileHash = result.FileHash,
                createdAt = result.CreatedAt
            });
        }
        catch (InvalidOperationException ex)
        {
            // Duplicate file registration.
            return Conflict(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            // Validation error.
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            // Unexpected server error.
            return StatusCode(500, new
            {
                message = "An unexpected error occurred.",
                error = ex.Message
            });
        }
    }


    // ---------------------------------------------------------
    // Get all registered file fingerprints.
    // ---------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAllFiles()
    {
        try
        {
            var files =
                await _fileIntegrityService.GetAllFilesAsync();

            return Ok(files);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred.",
                error = ex.Message
            });
        }
    }


    // ---------------------------------------------------------
    // Get one registered file fingerprint by ID.
    // ---------------------------------------------------------
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFileById(int id)
    {
        try
        {
            var file =
                await _fileIntegrityService.GetFileByIdAsync(id);

            if (file == null)
            {
                return NotFound(new
                {
                    message =
                        "The requested file fingerprint was not found."
                });
            }

            return Ok(file);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred.",
                error = ex.Message
            });
        }
    }


    // ---------------------------------------------------------
    // Check an uploaded file against a stored fingerprint.
    // ---------------------------------------------------------
    [HttpPost("check")]
    public async Task<IActionResult> CheckFileIntegrity(
        [FromForm] FileIntegrityCheckRequestDto request)
    {
        try
        {
            var result =
                await _fileIntegrityService
                    .CheckFileIntegrityAsync(request);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            // The selected database fingerprint doesn't exist.
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            // Validation error.
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred.",
                error = ex.Message
            });
        }
    }
}