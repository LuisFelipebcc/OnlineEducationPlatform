using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using PaymentBilling.Application.Services;
using PaymentBilling.Domain;
using PaymentBilling.Domain.Repositories;
using Xunit;

namespace PaymentBilling.Tests.Application.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _mediatorMock = new Mock<IMediator>();
            _paymentService = new PaymentService(_paymentRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task CriarPagamentoAsync_ComDadosValidos_DeveCriarPagamento()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var valor = 100m;
            var numeroCartao = "4111111111111111";
            var nomeTitular = "João Silva";
            var dataValidade = "12/25";
            var codigoSeguranca = "123";

            _paymentRepositoryMock
                .Setup(x => x.AdicionarAsync(It.IsAny<Payment>()))
                .Returns(Task.CompletedTask);

            _mediatorMock
                .Setup(x => x.Publish(It.IsAny<INotification>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var payment = await _paymentService.CriarPagamentoAsync(
                alunoId,
                cursoId,
                valor,
                numeroCartao,
                nomeTitular,
                dataValidade,
                codigoSeguranca);

            // Assert
            Assert.NotNull(payment);
            Assert.Equal(alunoId, payment.AlunoId);
            Assert.Equal(cursoId, payment.CursoId);
            Assert.Equal(valor, payment.Valor);
            Assert.Equal(PaymentStatus.Pendente, payment.Status);

            _paymentRepositoryMock.Verify(x => x.AdicionarAsync(It.IsAny<Payment>()), Times.Once);
            _mediatorMock.Verify(x => x.Publish(It.IsAny<INotification>(), default), Times.Once);
        }

        [Fact]
        public async Task ConfirmarPagamentoAsync_ComPagamentoExistente_DeveConfirmarPagamento()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            _paymentRepositoryMock
                .Setup(x => x.ObterPorIdAsync(paymentId))
                .ReturnsAsync(payment);

            _paymentRepositoryMock
                .Setup(x => x.AtualizarAsync(It.IsAny<Payment>()))
                .Returns(Task.CompletedTask);

            _mediatorMock
                .Setup(x => x.Publish(It.IsAny<INotification>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _paymentService.ConfirmarPagamentoAsync(paymentId, "TRANS123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(PaymentStatus.Confirmado, result.Status);
            Assert.Equal("TRANS123", result.CodigoTransacao);

            _paymentRepositoryMock.Verify(x => x.AtualizarAsync(It.IsAny<Payment>()), Times.Once);
            _mediatorMock.Verify(x => x.Publish(It.IsAny<INotification>(), default), Times.Once);
        }

        [Fact]
        public async Task ConfirmarPagamentoAsync_ComPagamentoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            _paymentRepositoryMock
                .Setup(x => x.ObterPorIdAsync(paymentId))
                .ReturnsAsync((Payment)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _paymentService.ConfirmarPagamentoAsync(paymentId, "TRANS123"));
        }

        [Fact]
        public async Task RejeitarPagamentoAsync_ComPagamentoExistente_DeveRejeitarPagamento()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            _paymentRepositoryMock
                .Setup(x => x.ObterPorIdAsync(paymentId))
                .ReturnsAsync(payment);

            _paymentRepositoryMock
                .Setup(x => x.AtualizarAsync(It.IsAny<Payment>()))
                .Returns(Task.CompletedTask);

            _mediatorMock
                .Setup(x => x.Publish(It.IsAny<INotification>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _paymentService.RejeitarPagamentoAsync(paymentId, "Cartão recusado");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(PaymentStatus.Rejeitado, result.Status);
            Assert.Equal("Cartão recusado", result.MensagemErro);

            _paymentRepositoryMock.Verify(x => x.AtualizarAsync(It.IsAny<Payment>()), Times.Once);
            _mediatorMock.Verify(x => x.Publish(It.IsAny<INotification>(), default), Times.Once);
        }

        [Fact]
        public async Task CancelarPagamentoAsync_ComPagamentoExistente_DeveCancelarPagamento()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            _paymentRepositoryMock
                .Setup(x => x.ObterPorIdAsync(paymentId))
                .ReturnsAsync(payment);

            _paymentRepositoryMock
                .Setup(x => x.AtualizarAsync(It.IsAny<Payment>()))
                .Returns(Task.CompletedTask);

            _mediatorMock
                .Setup(x => x.Publish(It.IsAny<INotification>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _paymentService.CancelarPagamentoAsync(paymentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(PaymentStatus.Cancelado, result.Status);

            _paymentRepositoryMock.Verify(x => x.AtualizarAsync(It.IsAny<Payment>()), Times.Once);
            _mediatorMock.Verify(x => x.Publish(It.IsAny<INotification>(), default), Times.Once);
        }
    }
}