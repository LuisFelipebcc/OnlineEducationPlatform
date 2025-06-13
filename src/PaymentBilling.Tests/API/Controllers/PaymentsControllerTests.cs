using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentBilling.API.Controllers;
using PaymentBilling.Application.Services;
using PaymentBilling.Domain;
using Xunit;

namespace PaymentBilling.Tests.API.Controllers
{
    public class PaymentsControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly PaymentsController _controller;

        public PaymentsControllerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _controller = new PaymentsController(_paymentServiceMock.Object);
        }

        [Fact]
        public async Task CriarPagamento_ComDadosValidos_DeveRetornarCreated()
        {
            // Arrange
            var request = new PaymentsController.CriarPagamentoRequest
            {
                AlunoId = Guid.NewGuid(),
                CursoId = Guid.NewGuid(),
                Valor = 100m,
                NumeroCartao = "4111111111111111",
                NomeTitular = "João Silva",
                DataValidade = "12/25",
                CodigoSeguranca = "123"
            };

            var payment = new Payment(
                request.AlunoId,
                request.CursoId,
                request.Valor,
                new Domain.ValueObjects.CardData(
                    request.NumeroCartao,
                    request.NomeTitular,
                    request.DataValidade,
                    request.CodigoSeguranca));

            _paymentServiceMock
                .Setup(x => x.CriarPagamentoAsync(
                    request.AlunoId,
                    request.CursoId,
                    request.Valor,
                    request.NumeroCartao,
                    request.NomeTitular,
                    request.DataValidade,
                    request.CodigoSeguranca))
                .ReturnsAsync(payment);

            // Act
            var result = await _controller.CriarPagamento(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Payment>(createdResult.Value);
            Assert.Equal(payment.Id, returnValue.Id);
            Assert.Equal(payment.AlunoId, returnValue.AlunoId);
            Assert.Equal(payment.CursoId, returnValue.CursoId);
        }

        [Fact]
        public async Task ObterPagamento_ComPagamentoExistente_DeveRetornarOk()
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

            _paymentServiceMock
                .Setup(x => x.ObterPagamentoAsync(paymentId))
                .ReturnsAsync(payment);

            // Act
            var result = await _controller.ObterPagamento(paymentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Payment>(okResult.Value);
            Assert.Equal(payment.Id, returnValue.Id);
        }

        [Fact]
        public async Task ObterPagamento_ComPagamentoInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            _paymentServiceMock
                .Setup(x => x.ObterPagamentoAsync(paymentId))
                .ReturnsAsync((Payment)null);

            // Act
            var result = await _controller.ObterPagamento(paymentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ConfirmarPagamento_ComPagamentoExistente_DeveRetornarOk()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var request = new PaymentsController.ConfirmarPagamentoRequest
            {
                CodigoTransacaoGateway = "TRANS123"
            };

            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            _paymentServiceMock
                .Setup(x => x.ConfirmarPagamentoAsync(paymentId, request.CodigoTransacaoGateway))
                .ReturnsAsync(payment);

            // Act
            var result = await _controller.ConfirmarPagamento(paymentId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Payment>(okResult.Value);
            Assert.Equal(payment.Id, returnValue.Id);
        }

        [Fact]
        public async Task RejeitarPagamento_ComPagamentoExistente_DeveRetornarOk()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var request = new PaymentsController.RejeitarPagamentoRequest
            {
                MensagemErro = "Cartão recusado"
            };

            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            _paymentServiceMock
                .Setup(x => x.RejeitarPagamentoAsync(paymentId, request.MensagemErro))
                .ReturnsAsync(payment);

            // Act
            var result = await _controller.RejeitarPagamento(paymentId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Payment>(okResult.Value);
            Assert.Equal(payment.Id, returnValue.Id);
        }

        [Fact]
        public async Task CancelarPagamento_ComPagamentoExistente_DeveRetornarOk()
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

            _paymentServiceMock
                .Setup(x => x.CancelarPagamentoAsync(paymentId))
                .ReturnsAsync(payment);

            // Act
            var result = await _controller.CancelarPagamento(paymentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Payment>(okResult.Value);
            Assert.Equal(payment.Id, returnValue.Id);
        }
    }
}