using PaymentBilling.Domain.Entities;

namespace PaymentBilling.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> GetByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<IEnumerable<Payment>> GetByAlunoIdAsync(Guid alunoId);
        Task<IEnumerable<Payment>> GetByMatriculaIdAsync(Guid matriculaId);
        Task<Payment> CreatePagamentoAsync(Payment pagamento);
        Task ConfirmarPagamentoAsync(Guid id);
        Task CancelarPagamentoAsync(Guid id, string motivo);
        Task ReembolsarPagamentoAsync(Guid id, string motivo);
    }
}