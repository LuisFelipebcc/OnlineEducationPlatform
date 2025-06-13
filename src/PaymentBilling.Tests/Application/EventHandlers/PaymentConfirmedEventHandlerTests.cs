using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentBilling.Application.EventHandlers;
using PaymentBilling.Domain.Events;
using Xunit;

namespace PaymentBilling.Tests.Application.EventHandlers
{
    public class PaymentConfirmedEventHandlerTests
    {
        private readonly Mock<ILogger<PaymentConfirmedEventHandler>> _loggerMock;
        private readonly PaymentConfirmedEventHandler _handler;

        public PaymentConfirmedEventHandlerTests()
        {
            _loggerMock = new Mock<ILogger<PaymentConfirmedEventHandler>>();
            _handler = new PaymentConfirmedEventHandler(_loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ComEventoValido_DeveProcessarEvento()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var codigoTransacaoGateway = "TRANS123";

            var @event = new PaymentConfirmedEvent(paymentId, alunoId, cursoId, codigoTransacaoGateway);

            // Act
            await _handler.Handle(@event, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(paymentId.ToString())),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}