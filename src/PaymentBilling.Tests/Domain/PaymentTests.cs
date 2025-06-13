using System;
using Xunit;
using PaymentBilling.Domain;
using PaymentBilling.Domain.ValueObjects;

namespace PaymentBilling.Tests.Domain
{
    public class PaymentTests
    {
        [Fact]
        public void CriarPagamento_ComDadosValidos_DeveCriarPagamentoPendente()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var valor = 100m;
            var dadosCartao = new CardData(
                "4111111111111111",
                "João Silva",
                "12/25",
                "123");

            // Act
            var payment = new Payment(alunoId, cursoId, valor, dadosCartao);

            // Assert
            Assert.Equal(alunoId, payment.AlunoId);
            Assert.Equal(cursoId, payment.CursoId);
            Assert.Equal(valor, payment.Valor);
            Assert.Equal(PaymentStatus.Pendente, payment.Status);
            Assert.NotNull(payment.CodigoTransacao);
            Assert.Single(payment.Eventos);
        }

        [Fact]
        public void CriarPagamento_ComValorZero_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var valor = 0m;
            var dadosCartao = new CardData(
                "4111111111111111",
                "João Silva",
                "12/25",
                "123");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Payment(alunoId, cursoId, valor, dadosCartao));
        }

        [Fact]
        public void ConfirmarPagamento_ComPagamentoPendente_DeveConfirmarPagamento()
        {
            // Arrange
            var payment = CriarPagamentoValido();
            var codigoTransacao = "TRANS123";

            // Act
            payment.Confirmar(codigoTransacao);

            // Assert
            Assert.Equal(PaymentStatus.Confirmado, payment.Status);
            Assert.Equal(codigoTransacao, payment.CodigoTransacao);
            Assert.NotNull(payment.DataProcessamento);
            Assert.Single(payment.Eventos);
        }

        [Fact]
        public void RejeitarPagamento_ComPagamentoPendente_DeveRejeitarPagamento()
        {
            // Arrange
            var payment = CriarPagamentoValido();
            var mensagemErro = "Cartão recusado";

            // Act
            payment.Rejeitar(mensagemErro);

            // Assert
            Assert.Equal(PaymentStatus.Rejeitado, payment.Status);
            Assert.Equal(mensagemErro, payment.MensagemErro);
            Assert.NotNull(payment.DataProcessamento);
            Assert.Single(payment.Eventos);
        }

        [Fact]
        public void CancelarPagamento_ComPagamentoPendente_DeveCancelarPagamento()
        {
            // Arrange
            var payment = CriarPagamentoValido();

            // Act
            payment.Cancelar();

            // Assert
            Assert.Equal(PaymentStatus.Cancelado, payment.Status);
            Assert.NotNull(payment.DataProcessamento);
            Assert.Single(payment.Eventos);
        }

        [Fact]
        public void ConfirmarPagamento_ComPagamentoConfirmado_DeveLancarExcecao()
        {
            // Arrange
            var payment = CriarPagamentoValido();
            payment.Confirmar("TRANS123");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => payment.Confirmar("TRANS456"));
        }

        private Payment CriarPagamentoValido()
        {
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var valor = 100m;
            var dadosCartao = new CardData(
                "4111111111111111",
                "João Silva",
                "12/25",
                "123");

            return new Payment(alunoId, cursoId, valor, dadosCartao);
        }
    }
}