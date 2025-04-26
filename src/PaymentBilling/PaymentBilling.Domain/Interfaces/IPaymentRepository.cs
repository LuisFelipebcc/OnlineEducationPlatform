using PaymentBilling.Domain.Entities;

namespace PaymentBilling.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(Guid id);
    Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Payment>> GetByCourseIdAsync(Guid courseId);
    Task<Payment> AddAsync(Payment payment);
    Task<Payment> UpdateAsync(Payment payment);
    Task<bool> DeleteAsync(Guid id);
}