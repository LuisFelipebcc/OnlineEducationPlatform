using ContentManagement.Application.Commands;
using ContentManagement.Application.Queries;
using ContentManagement.Domain.Aggregates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Curso>> CriarCurso([FromBody] CriarCursoCommand command)
        {
            var curso = await _mediator.Send(command);
            return CreatedAtAction(nameof(ObterPorId), new { id = curso.Id }, curso);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> ObterPorId(Guid id)
        {
            var query = new ObterCursoPorIdQuery { Id = id };
            var curso = await _mediator.Send(query);

            if (curso == null)
                return NotFound();

            return Ok(curso);
        }
    }
}