using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContentManagement.Domain.Entities;

namespace ContentManagement.Domain.Repositories
{
    public interface ICursoRepository
    {
        Task<Curso?> GetByIdAsync(Guid id);
        Task<IEnumerable<Curso>> GetAllAsync();
        // Poderia ter métodos para buscar cursos por instrutor, título, etc.
        // Ex: Task<IEnumerable<Curso>> GetByInstrutorIdAsync(Guid instrutorId);

        Task AddAsync(Curso curso);
        Task UpdateAsync(Curso curso);
        Task RemoveAsync(Curso curso);
    }
}