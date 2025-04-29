using PaymentBilling.Domain.Entities;

namespace PaymentBilling.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(Guid id);
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<IEnumerable<Payment>> GetByAlunoIdAsync(Guid alunoId);
    Task<IEnumerable<Payment>> GetByMatriculaIdAsync(Guid matriculaId);
    Task<Payment> AddAsync(Payment payment);
    Task<Payment> UpdateAsync(Payment payment);
    Task<bool> DeleteAsync(Guid id);
}
