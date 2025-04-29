using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContentManagement.Domain.Entities;

namespace ContentManagement.Domain.Repositories
{
    public interface IAulaRepository
    {
        Task<Aula> GetByIdAsync(Guid id);
        Task<IEnumerable<Aula>> GetByCursoIdAsync(Guid cursoId);
        Task<Aula> AddAsync(Aula aula);
        Task UpdateAsync(Aula aula);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}