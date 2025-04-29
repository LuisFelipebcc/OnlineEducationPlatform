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

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
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

    public async Task<IEnumerable<Payment>> GetByAlunoIdAsync(Guid alunoId)
    {
        return await _context.Payments
            .Where(p => p.AlunoId == alunoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByMatriculaIdAsync(Guid matriculaId)
    {
        return await _context.Payments
            .Where(p => p.MatriculaId == matriculaId)
            .ToListAsync();
    }
}