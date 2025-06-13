using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;

namespace OnlineEducationPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ou "api/matriculas" explicitamente
    [Authorize(Roles = "Aluno")] // Somente Alunos podem interagir com suas matrículas
    public class MatriculasController : ControllerBase
    {
        private readonly IAlunoAppService _alunoAppService;

        public MatriculasController(IAlunoAppService alunoAppService)
        {
            _alunoAppService = alunoAppService;
        }

        [HttpPost]
        public async Task<IActionResult> RealizarMatricula([FromBody] RealizarMatriculaDto realizarMatriculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var alunoIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier); // Ou JwtRegisteredClaimNames.Sub
            if (alunoIdClaim == null || !Guid.TryParse(alunoIdClaim.Value, out var alunoId))
            {
                return Unauthorized("ID do aluno não encontrado no token.");
            }

            try
            {
                var matricula = await _alunoAppService.RealizarMatriculaAsync(alunoId, realizarMatriculaDto);
                return matricula != null ? Ok(matricula) : BadRequest("Não foi possível realizar a matrícula.");
            }
            catch (KeyNotFoundException knfex)
            {
                return NotFound(knfex.Message); // Ex: Aluno não encontrado no StudentManagement
            }
            catch (InvalidOperationException ioex)
            {
                return Conflict(ioex.Message); // Ex: Aluno já matriculado
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMinhasMatriculas()
        {
            var alunoIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (alunoIdClaim == null || !Guid.TryParse(alunoIdClaim.Value, out var alunoId))
            {
                return Unauthorized("ID do aluno não encontrado no token.");
            }

            var matriculas = await _alunoAppService.GetMatriculasDoAlunoAsync(alunoId);
            return Ok(matriculas);
        }

        [HttpGet("{matriculaId}")]
        public async Task<IActionResult> GetMatriculaDetails(Guid matriculaId)
        {
            var alunoIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (alunoIdClaim == null || !Guid.TryParse(alunoIdClaim.Value, out var alunoId))
            {
                return Unauthorized("ID do aluno não encontrado no token.");
            }
            
            var matricula = await _alunoAppService.GetMatriculaDetailsAsync(alunoId, matriculaId);
            return matricula != null ? Ok(matricula) : NotFound("Matrícula não encontrada.");
        }

        [HttpDelete("{matriculaId}")]
        public async Task<IActionResult> CancelarMatricula(Guid matriculaId)
        {
            var alunoIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (alunoIdClaim == null || !Guid.TryParse(alunoIdClaim.Value, out var alunoId))
            {
                return Unauthorized("ID do aluno não encontrado no token.");
            }

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
            var alunoIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (alunoIdClaim == null || !Guid.TryParse(alunoIdClaim.Value, out var alunoId))
            {
                return Unauthorized("ID do aluno não encontrado no token.");
            }

            // O DTO MarcarAulaConcluidaDto não é estritamente necessário aqui, pois o aulaId vem da rota.
            // Mas se houvesse mais dados para passar no corpo, ele seria útil.

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