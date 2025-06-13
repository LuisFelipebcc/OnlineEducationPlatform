using Microsoft.EntityFrameworkCore;
using PaymentBilling.Domain.Entities;
using PaymentBilling.Domain.Repositories;
using PaymentBilling.Infrastructure.Persistence;

namespace PaymentBilling.Infrastructure.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PaymentBillingDbContext _context;

        public PagamentoRepository(PaymentBillingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Pagamento?> GetByIdAsync(Guid id)
        {
            return await _context.Pagamentos
                                 .Include(p => p.DadosCartaoUtilizado) // Se for um owned type
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pagamento>> GetByAlunoIdAsync(Guid alunoId)
        {
            return await _context.Pagamentos
                                 .Where(p => p.AlunoId == alunoId)
                                 .Include(p => p.DadosCartaoUtilizado)
                                 .ToListAsync();
        }

        public async Task<Pagamento?> GetByReferenciaPedidoIdAsync(Guid referenciaPedidoId)
        {
            return await _context.Pagamentos
                                 .Include(p => p.DadosCartaoUtilizado)
                                 .FirstOrDefaultAsync(p => p.ReferenciaPedidoId == referenciaPedidoId);
        }

        public async Task AddAsync(Pagamento pagamento)
        {
            await _context.Pagamentos.AddAsync(pagamento);
            // SaveChangesAsync será chamado pelo Unit of Work ou serviço de aplicação
        }

        public Task UpdateAsync(Pagamento pagamento)
        {
            _context.Entry(pagamento).State = EntityState.Modified;
            // Se DadosCartaoUtilizado for um owned type e puder ser modificado (o que não deveria ser comum),
            // o EF Core geralmente rastreia isso.
            return Task.CompletedTask;
        }
    }
}