using MediatR;
using Microsoft.Extensions.Logging;
using PaymentBilling.Application.DTOs;
using PaymentBilling.Domain.Entities;
using PaymentBilling.Domain.Repositories;
using PaymentBilling.Application.Events;
using PaymentBilling.Application.Interfaces;
using PaymentBilling.Infrastructure.Persistence;

namespace PaymentBilling.Application.Services
{
    public class PagamentoAppService : IPagamentoAppService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<PagamentoAppService> _logger;
        private readonly PaymentBillingDbContext _dbContext;

        public PagamentoAppService(
            IPagamentoRepository pagamentoRepository,
            IMediator mediator,
            ILogger<PagamentoAppService> logger,
            PaymentBillingDbContext dbContext)
        {
            _pagamentoRepository = pagamentoRepository ?? throw new ArgumentNullException(nameof(pagamentoRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<PagamentoDto?> ProcessarPagamentoAsync(CriarPagamentoDto criarPagamentoDto)
        {
            // Criação do pagamento sem dados sensíveis, apenas com o token
            var pagamento = new Pagamento(
                criarPagamentoDto.AlunoId,
                criarPagamentoDto.ReferenciaPedidoId,
                criarPagamentoDto.Valor,
                null // DadosCartao não é mais usado diretamente
            );

            await _pagamentoRepository.AddAsync(pagamento);

            pagamento.MarcarComoProcessando();
            await _dbContext.SaveChangesAsync();

            bool pagamentoAprovadoPeloGateway = SimularProcessamentoGateway(criarPagamentoDto.PaymentMethodToken);

            if (pagamentoAprovadoPeloGateway)
            {
                pagamento.Aprovar($"GATEWAY_TXN_{Guid.NewGuid().ToString().Substring(0, 8)}");
                _logger.LogInformation("Pagamento {PagamentoId} aprovado.", pagamento.Id);

                // Se o gateway retornar dados mascarados, use:
                // pagamento.AtualizarDetalhesCartaoGateway("****1234", "Nome Titular", "12/25");
            }
            else
            {
                pagamento.Rejeitar("Falha na autorização do método de pagamento.");
                _logger.LogWarning("Pagamento {PagamentoId} rejeitado.", pagamento.Id);
            }

            await _dbContext.SaveChangesAsync();

            await _mediator.Publish(new PagamentoProcessadoEvent(
                pagamento.Id,
                pagamento.AlunoId,
                pagamento.ReferenciaPedidoId,
                pagamento.Valor,
                pagamento.Status
            ));

            return MapToPagamentoDto(pagamento);
        }

        // Simulação do Gateway usando apenas o token
        private bool SimularProcessamentoGateway(string paymentMethodToken)
        {
            _logger.LogInformation("Simulando processamento do pagamento com token {Token}.", paymentMethodToken);
            Task.Delay(1000).Wait();
            // Simulação: tokens terminados em "0" são rejeitados
            return !paymentMethodToken.EndsWith("0");
        }

        public async Task<PagamentoDto?> GetPagamentoByIdAsync(Guid id)
        {
            var pagamento = await _pagamentoRepository.GetByIdAsync(id);
            return pagamento == null ? null : MapToPagamentoDto(pagamento);
        }

        public async Task<IEnumerable<PagamentoDto>> GetPagamentosByAlunoIdAsync(Guid alunoId)
        {
            var pagamentos = await _pagamentoRepository.GetByAlunoIdAsync(alunoId);
            return pagamentos.Select(MapToPagamentoDto);
        }

        public async Task<PagamentoDto?> ConfirmarPagamentoInternoAsync(Guid pagamentoId, string codigoTransacaoGateway)
        {
            var pagamento = await _pagamentoRepository.GetByIdAsync(pagamentoId);
            if (pagamento == null)
                throw new InvalidOperationException("Pagamento não encontrado");

            pagamento.Aprovar(codigoTransacaoGateway);
            await _pagamentoRepository.UpdateAsync(pagamento);
            await _dbContext.SaveChangesAsync();
            // Publicar evento PagamentoAprovadoEvent se necessário
            return MapToPagamentoDto(pagamento);
        }

        private PagamentoDto MapToPagamentoDto(Pagamento pagamento) => new PagamentoDto
        {
            Id = pagamento.Id,
            AlunoId = pagamento.AlunoId,
            ReferenciaPedidoId = pagamento.ReferenciaPedidoId,
            Valor = pagamento.Valor,
            DataCriacao = pagamento.DataCriacao,
            DataProcessamento = pagamento.DataProcessamento,
            Status = pagamento.Status.ToString(),
            NumeroCartaoMascarado = pagamento.DadosCartaoUtilizado?.NumeroCartaoMascarado,
            GatewayPagamentoId = pagamento.GatewayPagamentoId,
            MensagemErro = pagamento.MensagemErro
        };
    }
}