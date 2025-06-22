using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Interfaces
{
    public interface IMatriculaService
    {
        Task<Matricula> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Matricula>> ObterTodasAsync();
        Task<IEnumerable<Matricula>> ObterPorAlunoIdAsync(Guid alunoId);
        Task<IEnumerable<Matricula>> ObterPorCursoIdAsync(Guid cursoId);
        Task<Matricula> CriarMatriculaAsync(Matricula matricula);
        Task AtualizarMatriculaAsync(Matricula matricula);
        Task RemoverMatriculaAsync(Guid id);
    }
}