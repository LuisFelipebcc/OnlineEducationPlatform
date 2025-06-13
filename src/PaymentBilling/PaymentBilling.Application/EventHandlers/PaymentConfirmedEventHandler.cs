using MediatR;
using Microsoft.Extensions.Logging;
using PaymentBilling.Domain.Events;

namespace PaymentBilling.Application.EventHandlers
{
    public class PaymentConfirmedEventHandler : INotificationHandler<PaymentConfirmedEvent>
    {
        private readonly ILogger<PaymentConfirmedEventHandler> _logger;

        public PaymentConfirmedEventHandler(ILogger<PaymentConfirmedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PaymentConfirmedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Pagamento confirmado: {PaymentId} para o aluno {AlunoId} no curso {CourseId} com código de transação {CodigoTransacao}",
                notification.PaymentId,
                notification.AlunoId,
                notification.CourseId,
                notification.CodigoTransacaoGateway);

            // TODO: Enviar notificação para o aluno
            // TODO: Atualizar status da matrícula para ativo
            // TODO: Gerar comprovante de pagamento

            return Task.CompletedTask;
        }
    }
}