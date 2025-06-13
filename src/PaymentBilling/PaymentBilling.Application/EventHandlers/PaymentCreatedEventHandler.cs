using MediatR;
using Microsoft.Extensions.Logging;
using PaymentBilling.Domain.Events;

namespace PaymentBilling.Application.EventHandlers
{
    public class PaymentCreatedEventHandler : INotificationHandler<PaymentCreatedEvent>
    {
        private readonly ILogger<PaymentCreatedEventHandler> _logger;

        public PaymentCreatedEventHandler(ILogger<PaymentCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PaymentCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Pagamento criado: {PaymentId} para o aluno {AlunoId} no curso {CourseId} no valor de {Valor}",
                notification.PaymentId,
                notification.AlunoId,
                notification.CourseId,
                notification.Valor);

            // TODO: Implementar integração com gateway de pagamento
            // TODO: Enviar notificação para o aluno
            // TODO: Atualizar status da matrícula

            return Task.CompletedTask;
        }
    }
}