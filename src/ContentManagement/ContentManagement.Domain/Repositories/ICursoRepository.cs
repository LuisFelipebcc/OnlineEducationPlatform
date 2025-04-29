using ContentManagement.Domain.Aggregates;

namespace ContentManagement.Domain.Repositories
{
    public interface ICursoRepository
    {
        Task<Curso> GetByIdAsync(Guid id);
        Task<IEnumerable<Curso>> GetAllAsync();
        Task<Curso> AddAsync(Curso curso);
        Task UpdateAsync(Curso curso);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}