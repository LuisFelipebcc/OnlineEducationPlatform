using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentBilling.Domain;
using PaymentBilling.Domain.Enums;
using PaymentBilling.Infrastructure.Persistence;
using Xunit;

namespace PaymentBilling.Tests.Infrastructure.Persistence
{
    public class PaymentRepositoryTests : IDisposable
    {
        private readonly PaymentBillingDbContext _context;
        private readonly PaymentRepository _repository;

        public PaymentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<PaymentBillingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PaymentBillingDbContext(options);
            _repository = new PaymentRepository(_context);
        }

        [Fact]
        public async Task AdicionarAsync_ComPagamentoValido_DeveAdicionarPagamento()
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
            await _repository.AdicionarAsync(payment);

            // Assert
            var savedPayment = await _context.Payments.FindAsync(payment.Id);
            Assert.NotNull(savedPayment);
            Assert.Equal(payment.AlunoId, savedPayment.AlunoId);
            Assert.Equal(payment.CursoId, savedPayment.CursoId);
            Assert.Equal(payment.Valor, savedPayment.Valor);
        }

        [Fact]
        public async Task ObterPorIdAsync_ComPagamentoExistente_DeveRetornarPagamento()
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

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ObterPorIdAsync(payment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(payment.Id, result.Id);
            Assert.Equal(payment.AlunoId, result.AlunoId);
            Assert.Equal(payment.CursoId, result.CursoId);
        }

        [Fact]
        public async Task ObterPorIdAsync_ComPagamentoInexistente_DeveRetornarNull()
        {
            // Act
            var result = await _repository.ObterPorIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ObterPorAlunoAsync_ComPagamentosExistentes_DeveRetornarPagamentos()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var payments = new[]
            {
                new Payment(alunoId, Guid.NewGuid(), 100m, new Domain.ValueObjects.CardData("4111111111111111", "João Silva", "12/25", "123")),
                new Payment(alunoId, Guid.NewGuid(), 200m, new Domain.ValueObjects.CardData("4111111111111111", "João Silva", "12/25", "123")),
                new Payment(Guid.NewGuid(), Guid.NewGuid(), 300m, new Domain.ValueObjects.CardData("4111111111111111", "João Silva", "12/25", "123"))
            };

            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ObterPorAlunoAsync(alunoId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(alunoId, p.AlunoId));
        }

        [Fact]
        public async Task ObterPorStatusAsync_ComPagamentosExistentes_DeveRetornarPagamentos()
        {
            // Arrange
            var payments = new[]
            {
                new Payment(Guid.NewGuid(), Guid.NewGuid(), 100m, new Domain.ValueObjects.CardData("4111111111111111", "João Silva", "12/25", "123")),
                new Payment(Guid.NewGuid(), Guid.NewGuid(), 200m, new Domain.ValueObjects.CardData("4111111111111111", "João Silva", "12/25", "123"))
            };

            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ObterPorStatusAsync(PaymentStatus.Pendente);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(PaymentStatus.Pendente, p.Status));
        }

        [Fact]
        public async Task AtualizarAsync_ComPagamentoExistente_DeveAtualizarPagamento()
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

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            payment.Confirmar("TRANS123");
            await _repository.AtualizarAsync(payment);

            // Assert
            var updatedPayment = await _context.Payments.FindAsync(payment.Id);
            Assert.NotNull(updatedPayment);
            Assert.Equal(PaymentStatus.Confirmado, updatedPayment.Status);
            Assert.Equal("TRANS123", updatedPayment.CodigoTransacao);
        }

        [Fact]
        public async Task RemoverAsync_ComPagamentoExistente_DeveRemoverPagamento()
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

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            await _repository.RemoverAsync(payment.Id);

            // Assert
            var removedPayment = await _context.Payments.FindAsync(payment.Id);
            Assert.Null(removedPayment);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}