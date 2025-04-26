using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.API.Models;
using OnlineEducationPlatform.API.Services;

namespace OnlineEducationPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        try
        {
            var response = await _authService.Authenticate(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Credenciais inválidas" });
        }
    }

    [HttpGet("test-auth")]
    [Authorize]
    public IActionResult TestAuth()
    {
        return Ok(new { message = "Autenticado com sucesso!" });
    }

    [HttpGet("test-admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult TestAdmin()
    {
        return Ok(new { message = "Acesso de administrador confirmado!" });
    }
}