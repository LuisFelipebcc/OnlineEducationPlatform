using MediatR;
using Microsoft.Extensions.Logging; // Adicionado para Logger
using PaymentBilling.Application.DTOs; // Adicionado para DTOs
using PaymentBilling.Domain.Entities;
using PaymentBilling.Domain.Repositories;
using PaymentBilling.Domain.ValueObjects;
using PaymentBilling.Application.Events;
using PaymentBilling.Application.Interfaces;
using PaymentBilling.Infrastructure.Persistence; // Adicionado

namespace PaymentBilling.Application.Services
{
    // Renomeado de PaymentService para PagamentoAppService e IPaymentService para IPagamentoAppService
    public class PagamentoAppService : IPagamentoAppService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<PagamentoAppService> _logger; // Adicionado Logger
        private readonly PaymentBillingDbContext _dbContext; // Adicionado DbContext

        public PagamentoAppService( // Construtor corrigido para o nome da classe
            IPagamentoRepository pagamentoRepository,
            IMediator mediator,
            ILogger<PagamentoAppService> logger, // Adicionado Logger
            PaymentBilling.Infrastructure.Persistence.PaymentBillingDbContext dbContext) // Adicionado DbContext
        {
            _pagamentoRepository = pagamentoRepository ?? throw new ArgumentNullException(nameof(pagamentoRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Adicionado Logger
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); // Adicionado DbContext
        }

        // Método ajustado para usar CriarPagamentoDto
        public async Task<PagamentoDto?> ProcessarPagamentoAsync(CriarPagamentoDto criarPagamentoDto)
        {
            DadosCartao? dadosCartaoDominio = null;
            if (criarPagamentoDto.DadosCartao != null)
            {
                // ATENÇÃO: Em um sistema real, os dados do cartão não seriam passados assim.
                // Eles seriam tokenizados por um gateway de pagamento.
                // Esta é uma simplificação extrema.
                dadosCartaoDominio = new DadosCartao(
                    criarPagamentoDto.DadosCartao.NumeroCartao,
                    criarPagamentoDto.DadosCartao.NomeTitular,
                    criarPagamentoDto.DadosCartao.DataValidade);
            }

            var pagamento = new Pagamento( // Usando a entidade Pagamento
                criarPagamentoDto.AlunoId,
                criarPagamentoDto.ReferenciaPedidoId,
                criarPagamentoDto.Valor,
                dadosCartaoDominio);

            await _pagamentoRepository.AddAsync(pagamento);

            // Simulação da interação com um Gateway de Pagamento
            pagamento.MarcarComoProcessando();
            await _dbContext.SaveChangesAsync(); // Salva o estado "Processando"

            bool pagamentoAprovadoPeloGateway = SimularProcessamentoGateway(pagamento, criarPagamentoDto.DadosCartao);

            if (pagamentoAprovadoPeloGateway)
            {
                pagamento.Aprovar($"GATEWAY_TXN_{Guid.NewGuid().ToString().Substring(0, 8)}");
                _logger.LogInformation("Pagamento {PagamentoId} aprovado.", pagamento.Id);
            }
            else
            {
                pagamento.Rejeitar("Falha na autorização do cartão ou regra de negócio do gateway.");
                _logger.LogWarning("Pagamento {PagamentoId} rejeitado.", pagamento.Id);
            }

            await _dbContext.SaveChangesAsync(); // Salva o estado final

            // Publicar evento com o resultado do processamento
            await _mediator.Publish(new PagamentoProcessadoEvent(pagamento.Id, pagamento.AlunoId, pagamento.ReferenciaPedidoId, pagamento.Valor, pagamento.Status));

            return MapToPagamentoDto(pagamento);
        }

        // Simulação do Gateway
        private bool SimularProcessamentoGateway(Pagamento pagamento, DadosCartaoDto? dadosCartaoDto)
        {
            _logger.LogInformation("Simulando processamento do pagamento {PagamentoId} no gateway.", pagamento.Id);
            Task.Delay(1000).Wait();
            if (dadosCartaoDto == null) return false;
            return !dadosCartaoDto.NumeroCartao.EndsWith("0000");
        }

        public async Task<PagamentoDto?> GetPagamentoByIdAsync(Guid id)
        {
            var pagamento = await _pagamentoRepository.GetByIdAsync(id); // Usando o método correto do repositório
            return pagamento == null ? null : MapToPagamentoDto(pagamento);
        }

        public async Task<IEnumerable<PagamentoDto>> GetPagamentosByAlunoIdAsync(Guid alunoId)
        {
            var pagamentos = await _pagamentoRepository.GetByAlunoIdAsync(alunoId);
            return pagamentos.Select(MapToPagamentoDto);
        }

        // Métodos como Confirmar, Rejeitar, Cancelar seriam chamados por outros processos
        // (ex: um webhook do gateway de pagamento ou uma ação administrativa)
        // e não diretamente expostos da mesma forma que ProcessarPagamentoAsync.
        // Eles modificariam o estado de um pagamento existente.

        // Exemplo de como um método de confirmação poderia ser (chamado internamente ou por um handler de evento do gateway):
        public async Task<PagamentoDto?> ConfirmarPagamentoInternoAsync(Guid pagamentoId, string codigoTransacaoGateway)
        {
            var pagamento = await _pagamentoRepository.GetByIdAsync(pagamentoId);
            if (pagamento == null)
                throw new InvalidOperationException("Pagamento não encontrado");

            pagamento.Aprovar(codigoTransacaoGateway); // Método da entidade de domínio
            await _pagamentoRepository.UpdateAsync(pagamento);
            await _dbContext.SaveChangesAsync();
            // Publicar evento PagamentoAprovadoEvent
            // await _mediator.Publish(new PagamentoAprovadoEvent(pagamento.Id, ...));
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

        // Os métodos abaixo foram removidos pois a lógica de alteração de estado
        // deve ser encapsulada na entidade de domínio e chamada conforme necessário,
        // possivelmente por handlers de eventos ou outros serviços, não diretamente
        // como ações primárias do AppService da mesma forma que ProcessarPagamento.
        /*
        public async Task<Payment> RejeitarPagamentoAsync(Guid paymentId, string mensagemErro)
        {
            var payment = await _pagamentoRepository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new InvalidOperationException("Pagamento não encontrado");

            payment.Rejeitar(mensagemErro);
            await _pagamentoRepository.UpdateAsync(payment);

            foreach (var evento in payment.Eventos) // Supondo que a entidade Pagamento tenha uma coleção Eventos
            {
                await _mediator.Publish(evento);
            }
            return payment;
        }
        public async Task<Payment> CancelarPagamentoAsync(Guid paymentId)
        {
            var payment = await _pagamentoRepository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new InvalidOperationException("Pagamento não encontrado");

            payment.Cancelar();
            await _pagamentoRepository.UpdateAsync(payment);

            foreach (var evento in payment.Eventos) // Supondo que a entidade Pagamento tenha uma coleção Eventos
            {
                await _mediator.Publish(evento);
            }
            return payment;
        }
        */
    }
}