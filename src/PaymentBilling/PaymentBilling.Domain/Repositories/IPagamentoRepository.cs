using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentBilling.Domain.Entities;

namespace PaymentBilling.Domain.Repositories
{
    public interface IPagamentoRepository
    {
        Task<Pagamento?> GetByIdAsync(Guid id);
        Task<IEnumerable<Pagamento>> GetByAlunoIdAsync(Guid alunoId);
        Task<Pagamento?> GetByReferenciaPedidoIdAsync(Guid referenciaPedidoId);

        Task AddAsync(Pagamento pagamento);
        Task UpdateAsync(Pagamento pagamento);
        // A remoção de pagamentos geralmente não é uma prática comum,
        // mas sim o cancelamento ou reembolso.
    }
}