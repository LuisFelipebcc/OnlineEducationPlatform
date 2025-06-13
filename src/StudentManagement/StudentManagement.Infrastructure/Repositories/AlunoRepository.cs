using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Repositories;
using StudentManagement.Infrastructure.Persistence;

namespace StudentManagement.Infrastructure.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly StudentManagementDbContext _context;

        public AlunoRepository(StudentManagementDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Aluno?> GetByIdAsync(Guid id)
        {
            // Inclui as coleções que fazem parte do agregado Aluno
            return await _context.Alunos
                                 .Include(a => a.Matriculas)
                                 .ThenInclude(m => m.HistoricoAprendizado) // Se HistoricoAprendizado for uma entidade ou owned type complexo
                                 .Include(a => a.Certificados)
                                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Aluno>> GetAllAsync()
        {
            return await _context.Alunos
                                 .Include(a => a.Matriculas)
                                 .Include(a => a.Certificados)
                                 .ToListAsync();
        }

        public async Task AddAsync(Aluno aluno)
        {
            await _context.Alunos.AddAsync(aluno);
            // SaveChangesAsync será chamado pelo Unit of Work ou serviço de aplicação
        }

        public Task UpdateAsync(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified;
            // Marcar explicitamente as coleções se necessário, dependendo da complexidade das atualizações
            // e se elas são gerenciadas diretamente pelo EF Core através do rastreamento de mudanças.
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            // SaveChangesAsync será chamado pelo Unit of Work ou serviço de aplicação
            return Task.CompletedTask;
        }
    }
}