using PaymentBilling.Application.Interfaces;
using PaymentBilling.Domain.Entities;
using PaymentBilling.Domain.Interfaces;

namespace PaymentBilling.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment> GetByIdAsync(Guid id)
    {
        return await _paymentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _paymentRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Payment>> GetByAlunoIdAsync(Guid alunoId)
    {
        return await _paymentRepository.GetByAlunoIdAsync(alunoId);
    }

    public async Task<IEnumerable<Payment>> GetByMatriculaIdAsync(Guid matriculaId)
    {
        return await _paymentRepository.GetByMatriculaIdAsync(matriculaId);
    }

    public async Task<Payment> CreatePagamentoAsync(Payment pagamento)
    {
        return await _paymentRepository.AddAsync(pagamento);
    }

    public async Task ConfirmarPagamentoAsync(Guid id)
    {
        var pagamento = await _paymentRepository.GetByIdAsync(id);
        if (pagamento == null)
            throw new KeyNotFoundException("Pagamento não encontrado.");

        pagamento.ConfirmarPagamento();
        await _paymentRepository.UpdateAsync(pagamento);
    }

    public async Task CancelarPagamentoAsync(Guid id, string motivo)
    {
        var pagamento = await _paymentRepository.GetByIdAsync(id);
        if (pagamento == null)
            throw new KeyNotFoundException("Pagamento não encontrado.");

        pagamento.CancelarPagamento(motivo);
        await _paymentRepository.UpdateAsync(pagamento);
    }

    public async Task ReembolsarPagamentoAsync(Guid id, string motivo)
    {
        var pagamento = await _paymentRepository.GetByIdAsync(id);
        if (pagamento == null)
            throw new KeyNotFoundException("Pagamento não encontrado.");

        pagamento.ReembolsarPagamento(motivo);
        await _paymentRepository.UpdateAsync(pagamento);
    }
}
