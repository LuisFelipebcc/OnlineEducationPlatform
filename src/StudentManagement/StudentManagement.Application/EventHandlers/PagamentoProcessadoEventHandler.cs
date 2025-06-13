using MediatR;
using Microsoft.Extensions.Logging;
using PaymentBilling.Application.Events; // Evento do PaymentBilling
using PaymentBilling.Domain.Enums; // StatusPagamento do PaymentBilling
using StudentManagement.Application.Interfaces;

namespace StudentManagement.Application.EventHandlers
{
    public class PagamentoProcessadoEventHandler : INotificationHandler<PagamentoProcessadoEvent>
    {
        private readonly IAlunoAppService _alunoAppService;
        private readonly ILogger<PagamentoProcessadoEventHandler> _logger;

        public PagamentoProcessadoEventHandler(IAlunoAppService alunoAppService, ILogger<PagamentoProcessadoEventHandler> logger)
        {
            _alunoAppService = alunoAppService;
            _logger = logger;
        }

        public async Task Handle(PagamentoProcessadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Recebido PagamentoProcessadoEvent para PagamentoId: {PagamentoId}, ReferenciaPedidoId (MatriculaId): {MatriculaId}, Status: {Status}",
                notification.PagamentoId, notification.ReferenciaPedidoId, notification.Status);

            if (notification.Status == StatusPagamento.Aprovado)
            {
                // A ReferenciaPedidoId no PagamentoProcessadoEvent deve ser o MatriculaId
                await _alunoAppService.ConfirmarMatriculaAposPagamentoAsync(notification.ReferenciaPedidoId, notification.ValorPago);
                _logger.LogInformation("Matrícula {MatriculaId} confirmada após pagamento aprovado.", notification.ReferenciaPedidoId);
            }
            // Adicionar lógica para outros status de pagamento, se necessário (ex: Rejeitado)
        }
    }
}