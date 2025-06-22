using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;

namespace OnlineEducationPlatform.API.Controllers
{
    [ApiController]
    [Route("api/matriculas")]
    [Authorize(Roles = "Aluno")]
    public class MatriculasController : ControllerBase
    {
        private readonly IAlunoAppService _alunoAppService;

        public MatriculasController(IAlunoAppService alunoAppService)
        {
            _alunoAppService = alunoAppService;
        }

        private bool TryGetAlunoId(out Guid alunoId)
        {
            alunoId = Guid.Empty;
            var alunoIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return alunoIdClaim != null && Guid.TryParse(alunoIdClaim.Value, out alunoId);
        }

        [HttpPost]
        public async Task<IActionResult> RealizarMatricula([FromBody] RealizarMatriculaDto realizarMatriculaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetAlunoId(out var alunoId))
                return Unauthorized("ID do aluno não encontrado no token.");

            try
            {
                var matricula = await _alunoAppService.RealizarMatriculaAsync(alunoId, realizarMatriculaDto);
                return matricula != null ? Ok(matricula) : BadRequest("Falha ao criar matrícula.");
            }
            catch (KeyNotFoundException knfex)
            {
                return NotFound(knfex.Message);
            }
            catch (InvalidOperationException ioex)
            {
                return Conflict(ioex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMinhasMatriculas()
        {
            if (!TryGetAlunoId(out var alunoId))
                return Unauthorized("ID do aluno não encontrado no token.");

            var matriculas = await _alunoAppService.GetMatriculasDoAlunoAsync(alunoId);
            return Ok(matriculas);
        }

        [HttpGet("{matriculaId}")]
        public async Task<IActionResult> GetDetalhesMatricula(Guid matriculaId)
        {
            if (!TryGetAlunoId(out var alunoId))
                return Unauthorized("ID do aluno não encontrado no token.");

            var matricula = await _alunoAppService.GetMatriculaDetailsAsync(alunoId, matriculaId);
            return matricula != null ? Ok(matricula) : NotFound("Matrícula não encontrada.");
        }

        [HttpDelete("{matriculaId}")]
        public async Task<IActionResult> CancelarMatricula(Guid matriculaId)
        {
            if (!TryGetAlunoId(out var alunoId))
                return Unauthorized("ID do aluno não encontrado no token.");

            try
            {
                await _alunoAppService.CancelarMatriculaAsync(alunoId, matriculaId);
                return NoContent();
            }
            catch (KeyNotFoundException knfex)
            {
                return NotFound(knfex.Message);
            }
            catch (InvalidOperationException ioex)
            {
                return Conflict(ioex.Message);
            }
        }

        [HttpPost("{matriculaId}/aulas/{aulaId}/concluir")]
        public async Task<IActionResult> MarcarAulaComoConcluida(Guid matriculaId, Guid aulaId)
        {
            if (!TryGetAlunoId(out var alunoId))
                return Unauthorized("ID do aluno não encontrado no token.");

            try
            {
                await _alunoAppService.MarcarAulaComoConcluidaAsync(alunoId, matriculaId, aulaId);
                return Ok("Aula marcada como concluída.");
            }
            catch (KeyNotFoundException knfex)
            {
                return NotFound(knfex.Message);
            }
            catch (InvalidOperationException ioex)
            {
                return BadRequest(ioex.Message);
            }
        }
    }
}