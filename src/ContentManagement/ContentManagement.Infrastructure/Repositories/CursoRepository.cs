using ContentManagement.Domain.Aggregates;
using ContentManagement.Domain.Repositories;
using ContentManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Repositories;

public class CursoRepository : ICursoRepository
{
    private readonly ContentManagementContext _context;

    public CursoRepository(ContentManagementContext context)
    {
        _context = context;
    }

    public async Task<Curso> GetByIdAsync(Guid id)
    {
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

    public async Task<Curso> AddAsync(Curso curso)
    {
        await _context.Cursos.AddAsync(curso);
        await _context.SaveChangesAsync();
        return curso;
    }

    public async Task UpdateAsync(Curso curso)
    {
        _context.Entry(curso).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var curso = await GetByIdAsync(id);
        if (curso != null)
        {
            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Cursos.AnyAsync(c => c.Id == id);
    }
}
}