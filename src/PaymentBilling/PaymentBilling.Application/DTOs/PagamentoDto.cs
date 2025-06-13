using System;
using PaymentBilling.Domain.Enums; // Para StatusPagamento

namespace PaymentBilling.Application.DTOs
{
    public class PagamentoDto
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public Guid ReferenciaPedidoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public string Status { get; set; } // String para facilitar a exibição
        public string? NumeroCartaoMascarado { get; set; } // Apenas o mascarado para exibição
        public string? GatewayPagamentoId { get; set; }
        public string? MensagemErro { get; set; }
    }
}