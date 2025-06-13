using System;
using FluentValidation.TestHelper;
using PaymentBilling.Application.Validators;
using PaymentBilling.Domain;
using PaymentBilling.Domain.Enums;
using Xunit;

namespace PaymentBilling.Tests.Application.Validators
{
    public class PaymentValidatorTests
    {
        private readonly PaymentValidator _validator;

        public PaymentValidatorTests()
        {
            _validator = new PaymentValidator();
        }

        [Fact]
        public void Validate_ComPagamentoValido_DevePassar()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            // Act
            var result = _validator.TestValidate(payment);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ComAlunoIdVazio_DeveFalhar()
        {
            // Arrange
            var payment = new Payment(
                Guid.Empty,
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            // Act
            var result = _validator.TestValidate(payment);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AlunoId);
        }

        [Fact]
        public void Validate_ComCursoIdVazio_DeveFalhar()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.Empty,
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            // Act
            var result = _validator.TestValidate(payment);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CursoId);
        }

        [Fact]
        public void Validate_ComValorZero_DeveFalhar()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                0m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            // Act
            var result = _validator.TestValidate(payment);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Valor);
        }

        [Fact]
        public void Validate_ComValorNegativo_DeveFalhar()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                -100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            // Act
            var result = _validator.TestValidate(payment);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Valor);
        }

        [Fact]
        public void Validate_ComDadosCartaoNulo_DeveFalhar()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                null);

            // Act
            var result = _validator.TestValidate(payment);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DadosCartao);
        }

        [Fact]
        public void Validate_ComPagamentoConfirmadoSemCodigoTransacao_DeveFalhar()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            payment.Confirmar(null);

            // Act
            var result = _validator.TestValidate(payment);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CodigoTransacao);
        }

        [Fact]
        public void Validate_ComPagamentoRejeitadoSemMensagemErro_DeveFalhar()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100m,
                new Domain.ValueObjects.CardData(
                    "4111111111111111",
                    "João Silva",
                    "12/25",
                    "123"));

            payment.Rejeitar(null);

            // Act
            var result = _validator.TestValidate(payment);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MensagemErro);
        }
    }
}