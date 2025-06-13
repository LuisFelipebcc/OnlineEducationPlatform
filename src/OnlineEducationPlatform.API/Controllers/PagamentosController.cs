using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentBilling.Application.DTOs;
using PaymentBilling.Application.Interfaces;

namespace OnlineEducationPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Geralmente, operações de pagamento exigem autenticação
    public class PagamentosController : ControllerBase
    {
        private readonly IPagamentoAppService _pagamentoAppService;

        public PagamentosController(IPagamentoAppService pagamentoAppService)
        {
            _pagamentoAppService = pagamentoAppService;
        }

        [HttpPost("processar")]
        [Authorize(Roles = "Aluno")] // Somente um Aluno pode iniciar um pagamento para si
        public async Task<IActionResult> ProcessarPagamento([FromBody] CriarPagamentoDto criarPagamentoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var alunoIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (alunoIdClaim == null || !Guid.TryParse(alunoIdClaim.Value, out var alunoIdAutenticado) || alunoIdAutenticado != criarPagamentoDto.AlunoId)
            {
                return Unauthorized("O AlunoId no DTO não corresponde ao usuário autenticado ou o token é inválido.");
            }

            try
            {
                var resultadoPagamento = await _pagamentoAppService.ProcessarPagamentoAsync(criarPagamentoDto);
                if (resultadoPagamento == null)
                {
                    return BadRequest("Falha ao processar o pagamento.");
                }
                // Você pode querer retornar CreatedAtAction se tiver um endpoint GetPagamentoById
                return Ok(resultadoPagamento);
            }
            catch (Exception ex)
            {
                // Logar a exceção
                return StatusCode(500, $"Erro interno ao processar pagamento: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagamentoById(Guid id)
        {
            // Adicionar lógica para verificar se o usuário logado (aluno ou admin) tem permissão para ver este pagamento.
            var pagamento = await _pagamentoAppService.GetPagamentoByIdAsync(id);
            return pagamento != null ? Ok(pagamento) : NotFound("Pagamento não encontrado.");
        }

        [HttpGet("meus-pagamentos")]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> GetMeusPagamentos()
        {
            var alunoIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (alunoIdClaim == null || !Guid.TryParse(alunoIdClaim.Value, out var alunoIdAutenticado))
            {
                return Unauthorized("ID do aluno não encontrado no token.");
            }

            var pagamentos = await _pagamentoAppService.GetPagamentosByAlunoIdAsync(alunoIdAutenticado);
            return Ok(pagamentos);
        }

        // TODO: Adicionar endpoints para Admin gerenciar/visualizar pagamentos (se necessário).
        // Ex: [HttpGet("admin/aluno/{alunoId}")]
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> GetPagamentosDoAlunoPorAdmin(Guid alunoId) { ... _pagamentoAppService.GetPagamentosByAlunoIdAsync(alunoId) ... }
    }
}