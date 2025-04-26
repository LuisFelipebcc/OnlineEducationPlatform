using ContentManagement.Application.Commands;
using ContentManagement.Application.DTOs;
using ContentManagement.Application.Extensions;
using ContentManagement.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CursosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICursoRepository _cursoRepository;

    public CursosController(IMediator mediator, ICursoRepository cursoRepository)
    {
        _mediator = mediator;
        _cursoRepository = cursoRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CursoDTO>> Criar([FromBody] CriarCursoDTO dto)
    {
        var command = new CriarCursoCommand { DTO = dto };
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CursoDTO>> ObterPorId(Guid id)
    {
        var curso = await _cursoRepository.ObterPorIdAsync(id);
        if (curso == null)
            return NotFound();

        return Ok(curso.ToDTO());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CursoDTO>>> ObterTodos()
    {
        var cursos = await _cursoRepository.ObterTodosAsync();
        return Ok(cursos.Select(c => c.ToDTO()));
    }

    [HttpGet("ativos")]
    public async Task<ActionResult<IEnumerable<CursoDTO>>> ObterAtivos()
    {
        var cursos = await _cursoRepository.ObterAtivosAsync();
        return Ok(cursos.Select(c => c.ToDTO()));
    }
}