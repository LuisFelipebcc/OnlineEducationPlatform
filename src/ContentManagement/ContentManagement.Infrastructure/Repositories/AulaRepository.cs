using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContentManagement.Domain.Entities;
using ContentManagement.Domain.Repositories;
using ContentManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Repositories
{
    public class AulaRepository : IAulaRepository
    {
        private readonly ContentManagementContext _context;

        public AulaRepository(ContentManagementContext context)
        {
            _context = context;
        }

        public async Task<Aula> GetByIdAsync(Guid id)
        {
            return await _context.Aulas
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Aula>> GetByCursoIdAsync(Guid cursoId)
        {
            return await _context.Aulas
                .Where(a => a.CursoId == cursoId)
                .OrderBy(a => a.Ordem)
                .ToListAsync();
        }

        public async Task<Aula> AddAsync(Aula aula)
        {
            await _context.Aulas.AddAsync(aula);
            await _context.SaveChangesAsync();
            return aula;
        }

        public async Task UpdateAsync(Aula aula)
        {
            _context.Entry(aula).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var aula = await GetByIdAsync(id);
            if (aula != null)
            {
                _context.Aulas.Remove(aula);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Aulas.AnyAsync(a => a.Id == id);
        }
    }
}