using PaymentBilling.Domain.Entities;
using PaymentBilling.Domain.Interfaces;

namespace PaymentBilling.Application.Services;

public class PaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment> CreatePaymentAsync(Guid userId, Guid courseId, decimal amount, PaymentMethod method)
    {
        var payment = new Payment(userId, courseId, amount, method);
        return await _paymentRepository.AddAsync(payment);
    }

    public async Task<Payment> GetPaymentByIdAsync(Guid id)
    {
        return await _paymentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetUserPaymentsAsync(Guid userId)
    {
        return await _paymentRepository.GetByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Payment>> GetCoursePaymentsAsync(Guid courseId)
    {
        return await _paymentRepository.GetByCourseIdAsync(courseId);
    }

    public async Task<Payment> ProcessPaymentAsync(Guid paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
            throw new InvalidOperationException("Payment not found");

        // Simular processamento de pagamento
        // Em um cenário real, aqui seria feita a integração com o gateway de pagamento
        await Task.Delay(1000); // Simular delay de processamento

        payment.MarkAsPaid();
        return await _paymentRepository.UpdateAsync(payment);
    }
}