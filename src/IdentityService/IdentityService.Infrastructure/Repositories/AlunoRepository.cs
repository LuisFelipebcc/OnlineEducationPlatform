using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly IdentityDbContext _context;

        public AlunoRepository(IdentityDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Aluno?> GetByIdAsync(Guid id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        public async Task AddAsync(Aluno aluno)
        {
            await _context.Alunos.AddAsync(aluno);
        }

        public Task UpdateAsync(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
            // Ou _context.Entry(aluno).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            return Task.CompletedTask;
        }
    }
}