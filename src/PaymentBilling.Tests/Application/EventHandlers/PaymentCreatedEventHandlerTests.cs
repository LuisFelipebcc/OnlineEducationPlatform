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
    public class PaymentCreatedEventHandlerTests
    {
        private readonly Mock<ILogger<PaymentCreatedEventHandler>> _loggerMock;
        private readonly PaymentCreatedEventHandler _handler;

        public PaymentCreatedEventHandlerTests()
        {
            _loggerMock = new Mock<ILogger<PaymentCreatedEventHandler>>();
            _handler = new PaymentCreatedEventHandler(_loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ComEventoValido_DeveProcessarEvento()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var valor = 100m;

            var @event = new PaymentCreatedEvent(paymentId, alunoId, cursoId, valor);

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