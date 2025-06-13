using MediatR;
using Shared.Domain;

namespace PaymentBilling.Domain.Events
{
    public class PaymentConfirmedEvent : DomainEvent, INotification
    {
        public Guid PagamentoId { get; }
        public Guid AlunoId { get; }
        public Guid CourseId { get; }
        public string CodigoTransacaoGateway { get; }
        public DateTime DataConfirmacao { get; }

        public PaymentConfirmedEvent(
            Guid pagamentoId,
            Guid alunoId,
            Guid cursoId,
            string codigoTransacaoGateway)
        {
            PagamentoId = pagamentoId;
            AlunoId = alunoId;
            CourseId = cursoId;
            CodigoTransacaoGateway = codigoTransacaoGateway;
            DataConfirmacao = DateTime.UtcNow;
        }
    }
}