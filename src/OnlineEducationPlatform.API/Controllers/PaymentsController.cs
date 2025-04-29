using Microsoft.AspNetCore.Mvc;
using PaymentBilling.Application.Interfaces;
using PaymentBilling.Domain.Entities;

namespace OnlineEducationPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        public PaymentsController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetAll()
        {
            var pagamentos = await _pagamentoService.GetAllAsync();
            return Ok(pagamentos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> GetById(Guid id)
        {
            var pagamento = await _pagamentoService.GetByIdAsync(id);
            if (pagamento == null)
            {
                return NotFound();
            }
            return Ok(pagamento);
        }

        [HttpGet("aluno/{alunoId}")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetByAlunoId(Guid alunoId)
        {
            var pagamentos = await _pagamentoService.GetByAlunoIdAsync(alunoId);
            return Ok(pagamentos);
        }

        [HttpGet("matricula/{matriculaId}")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetByMatriculaId(Guid matriculaId)
        {
            var pagamentos = await _pagamentoService.GetByMatriculaIdAsync(matriculaId);
            return Ok(pagamentos);
        }

        [HttpPost]
        public async Task<ActionResult<Pagamento>> Create(Pagamento pagamento)
        {
            var novoPagamento = await _pagamentoService.CreatePagamentoAsync(pagamento);
            return CreatedAtAction(nameof(GetById), new { id = novoPagamento.Id }, novoPagamento);
        }

        [HttpPost("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(Guid id)
        {
            await _pagamentoService.ConfirmarPagamentoAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(Guid id, [FromBody] string motivo)
        {
            await _pagamentoService.CancelarPagamentoAsync(id, motivo);
            return NoContent();
        }

        [HttpPost("{id}/reembolsar")]
        public async Task<IActionResult> Reembolsar(Guid id, [FromBody] string motivo)
        {
            await _pagamentoService.ReembolsarPagamentoAsync(id, motivo);
            return NoContent();
        }
    }
}
