using ContentManagement.Application.DTOs;
using ContentManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineEducationPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly ICursoAppService _cursoAppService;

        public CursosController(ICursoAppService cursoAppService)
        {
            _cursoAppService = cursoAppService;
        }

        [HttpGet]
        [AllowAnonymous] // Todos podem listar cursos
        public async Task<IActionResult> GetAllCursos()
        {
            var cursos = await _cursoAppService.GetAllCursosAsync();
            return Ok(cursos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Todos podem ver detalhes de um curso
        public async Task<IActionResult> GetCursoById(Guid id)
        {
            var curso = await _cursoAppService.GetCursoByIdAsync(id);
            return curso != null ? Ok(curso) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Somente Admin pode criar cursos
        public async Task<IActionResult> CreateCurso([FromBody] CriarCursoDto criarCursoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obter o ID do instrutor (Admin) a partir do token JWT
            var instrutorIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier); // Ou JwtRegisteredClaimNames.Sub
            if (instrutorIdClaim == null || !Guid.TryParse(instrutorIdClaim.Value, out var instrutorId))
            {
                return Unauthorized("ID do instrutor não encontrado no token.");
            }

            var cursoCriado = await _cursoAppService.CreateCursoAsync(criarCursoDto, instrutorId);
            return CreatedAtAction(nameof(GetCursoById), new { id = cursoCriado.Id }, cursoCriado);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Somente Admin pode atualizar cursos
        public async Task<IActionResult> UpdateCurso(Guid id, [FromBody] AtualizarCursoDto atualizarCursoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _cursoAppService.UpdateCursoAsync(id, atualizarCursoDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Somente Admin pode deletar cursos
        public async Task<IActionResult> DeleteCurso(Guid id)
        {
            try
            {
                await _cursoAppService.DeleteCursoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException) // Se o serviço lançar KeyNotFoundException quando não encontrar
            {
                return NotFound();
            }
        }

        // Endpoints para Aulas (exemplo)
        [HttpPost("{cursoId}/aulas")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAula(Guid cursoId, [FromBody] CriarAulaDto criarAulaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var aula = await _cursoAppService.AddAulaToCursoAsync(cursoId, criarAulaDto);
            // Poderia retornar CreatedAtAction se tivesse um endpoint GetAulaById
            return Ok(aula);
        }
    }
}