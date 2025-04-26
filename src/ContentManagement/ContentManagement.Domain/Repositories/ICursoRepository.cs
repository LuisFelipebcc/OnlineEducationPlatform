using ContentManagement.Domain.Aggregates;

namespace ContentManagement.Domain.Repositories;

public interface ICursoRepository
{
    Task<Curso> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Curso>> ObterTodosAsync();
    Task<IEnumerable<Curso>> ObterAtivosAsync();
    Task AdicionarAsync(Curso curso);
    Task AtualizarAsync(Curso curso);
    Task RemoverAsync(Curso curso);
}