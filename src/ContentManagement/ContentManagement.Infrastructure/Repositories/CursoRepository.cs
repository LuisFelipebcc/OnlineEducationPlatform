using ContentManagement.Domain.Entities;
using ContentManagement.Domain.Repositories;
using ContentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ContentManagementDbContext _context;

        public CursoRepository(ContentManagementDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Curso?> GetByIdAsync(Guid id)
        {
            // Inclui as Aulas ao buscar um Curso, pois Aulas são parte do agregado Curso.
            return await _context.Cursos
                                 .Include(c => c.Aulas)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Curso>> GetAllAsync()
        {
            return await _context.Cursos
                                 .Include(c => c.Aulas)
                                 .ToListAsync();
        }

        public async Task AddAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            // O SaveChangesAsync será chamado pela unidade de trabalho (Unit of Work) ou pelo serviço de aplicação.
        }

        public Task UpdateAsync(Curso curso)
        {
            // O EF Core rastreia mudanças em entidades carregadas.
            // Se o curso foi buscado e modificado, o SaveChangesAsync persistirá as alterações.
            // Se for uma entidade desconectada, pode ser necessário _context.Cursos.Update(curso);
            _context.Entry(curso).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Curso curso)
        {
            _context.Cursos.Remove(curso);
            // O SaveChangesAsync será chamado pela unidade de trabalho ou serviço de aplicação.
            return Task.CompletedTask;
        }
    }
}