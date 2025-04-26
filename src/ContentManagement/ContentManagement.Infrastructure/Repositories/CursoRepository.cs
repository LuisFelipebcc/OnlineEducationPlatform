using ContentManagement.Domain.Aggregates;
using ContentManagement.Domain.Repositories;
using ContentManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Repositories;

public class CursoRepository : ICursoRepository
{
    private readonly ContentManagementDbContext _context;

    public CursoRepository(ContentManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Curso> ObterPorIdAsync(Guid id)
    {
        return await _context.Cursos
            .Include(c => c.Aulas)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Curso>> ObterTodosAsync()
    {
        return await _context.Cursos
            .Include(c => c.Aulas)
            .ToListAsync();
    }

    public async Task<IEnumerable<Curso>> ObterAtivosAsync()
    {
        return await _context.Cursos
            .Include(c => c.Aulas)
            .Where(c => c.Ativo)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Curso curso)
    {
        await _context.Cursos.AddAsync(curso);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Curso curso)
    {
        _context.Cursos.Update(curso);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Curso curso)
    {
        _context.Cursos.Remove(curso);
        await _context.SaveChangesAsync();
    }
}