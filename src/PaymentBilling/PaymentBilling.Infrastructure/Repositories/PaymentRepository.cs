using Microsoft.EntityFrameworkCore;
using PaymentBilling.Domain.Entities;
using PaymentBilling.Domain.Interfaces;
using PaymentBilling.Infrastructure.Context;

namespace PaymentBilling.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentBillingDbContext _context;

    public PaymentRepository(PaymentBillingDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> GetByIdAsync(Guid id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Payments
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByCourseIdAsync(Guid courseId)
    {
        return await _context.Payments
            .Where(p => p.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null)
            return false;

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
        return true;
    }
}