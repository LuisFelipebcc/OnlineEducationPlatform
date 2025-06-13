using System;
using PaymentBilling.Domain.Enums;
using PaymentBilling.Domain.ValueObjects;
using MediatR; // Para INotification, se seus eventos herdarem disso
// using Shared.Domain; // Se tiver uma classe base AggregateRoot

namespace PaymentBilling.Domain.Entities
{
    public class Pagamento // : AggregateRoot
    {
        public Guid Id { get; private set; }
        public Guid AlunoId { get; private set; }
        public Guid ReferenciaPedidoId { get; private set; } // Ex: MatriculaId ou CursoId
        public decimal Valor { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataProcessamento { get; private set; }
        public StatusPagamento Status { get; private set; }
        public DadosCartao? DadosCartaoUtilizado { get; private set; } // Pode ser nulo se for outro método de pagamento
        public string? GatewayPagamentoId { get; private set; } // ID da transação no gateway
        public string? MensagemErro { get; private set; }

        private readonly List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();


        // Construtor para EF Core
        private Pagamento() { }

        public Pagamento(Guid alunoId, Guid referenciaPedidoId, decimal valor, DadosCartao? dadosCartao)
        {
            if (alunoId == Guid.Empty) throw new ArgumentException("AlunoId é obrigatório.", nameof(alunoId));
            if (referenciaPedidoId == Guid.Empty) throw new ArgumentException("ReferenciaPedidoId é obrigatório.", nameof(referenciaPedidoId));
            if (valor <= 0) throw new ArgumentOutOfRangeException(nameof(valor), "Valor deve ser positivo.");

            Id = Guid.NewGuid();
            AlunoId = alunoId;
            ReferenciaPedidoId = referenciaPedidoId;
            Valor = valor;
            DataCriacao = DateTime.UtcNow;
            Status = StatusPagamento.Pendente;
            DadosCartaoUtilizado = dadosCartao; // Pode ser nulo

            // Exemplo de como adicionar um evento (PagamentoCriadoEvent precisaria ser definido)
            // AddDomainEvent(new PagamentoCriadoEvent(this.Id, this.AlunoId, this.ReferenciaPedidoId, this.Valor));
        }

        public void MarcarComoProcessando()
        {
            if (Status != StatusPagamento.Pendente)
                throw new InvalidOperationException("Pagamento só pode ser marcado como processando se estiver pendente.");
            Status = StatusPagamento.Processando;
            DataProcessamento = DateTime.UtcNow;
        }

        public void Aprovar(string gatewayPagamentoId)
        {
            if (Status != StatusPagamento.Processando && Status != StatusPagamento.Pendente) // Alguns gateways podem aprovar direto
                throw new InvalidOperationException("Pagamento não pode ser aprovado neste estado.");
            Status = StatusPagamento.Aprovado;
            GatewayPagamentoId = gatewayPagamentoId ?? throw new ArgumentNullException(nameof(gatewayPagamentoId));
            DataProcessamento = DataProcessamento ?? DateTime.UtcNow;
            // Exemplo:
            // AddDomainEvent(new PagamentoAprovadoEvent(this.Id, this.AlunoId, this.ReferenciaPedidoId));
        }

        public void Rejeitar(string mensagemErro)
        {
            if (Status != StatusPagamento.Processando && Status != StatusPagamento.Pendente)
                throw new InvalidOperationException("Pagamento não pode ser rejeitado neste estado.");
            Status = StatusPagamento.Rejeitado;
            MensagemErro = mensagemErro;
            DataProcessamento = DataProcessamento ?? DateTime.UtcNow;
            // Exemplo:
            // AddDomainEvent(new PagamentoRejeitadoEvent(this.Id, this.AlunoId, this.ReferenciaPedidoId, mensagemErro));
        }

        public void Cancelar()
        {
            // Adicionar lógica de negócio para cancelamento (ex: só pode cancelar se pendente ou aprovado e não processado)
            Status = StatusPagamento.Cancelado;
            // Exemplo:
            // AddDomainEvent(new PagamentoCanceladoEvent(this.Id));
        }

        // Método para atualizar detalhes do cartão com informações seguras do gateway
        public void AtualizarDetalhesCartaoGateway(string numeroCartaoMascarado, string nomeTitular, string dataValidade)
        {
            DadosCartaoUtilizado = new DadosCartao(numeroCartaoMascarado, nomeTitular, dataValidade);
        }

        protected void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}