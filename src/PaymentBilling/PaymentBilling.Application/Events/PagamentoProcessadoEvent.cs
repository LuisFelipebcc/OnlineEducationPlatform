using System;
using MediatR;
using PaymentBilling.Domain.Enums;

namespace PaymentBilling.Application.Events
{
    public class PagamentoProcessadoEvent : INotification
    {
        public Guid PagamentoId { get; }
        public Guid AlunoId { get; }
        public Guid ReferenciaPedidoId { get; } // Ex: MatriculaId
        public decimal ValorPago { get; }
        public StatusPagamento Status { get; }

        public PagamentoProcessadoEvent(Guid pagamentoId, Guid alunoId, Guid referenciaPedidoId, decimal valorPago, StatusPagamento status)
        {
            PagamentoId = pagamentoId;
            AlunoId = alunoId;
            ReferenciaPedidoId = referenciaPedidoId;
            ValorPago = valorPago;
            Status = status;
        }
    }
}