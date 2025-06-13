using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioDto registrarUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _authenticationService.RegistrarAsync(registrarUsuarioDto);
            return resultado != null ? Ok(resultado) : BadRequest("Falha ao registrar usuário. O email pode já estar em uso ou o tipo de persona é inválido.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _authenticationService.LoginAsync(loginDto);
            return resultado != null ? Ok(resultado) : Unauthorized("Email ou senha inválidos.");
        }
    }
}