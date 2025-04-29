using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Interfaces
{
    public interface IMatriculaService
    {
        Task<Matricula> GetByIdAsync(int id);
        Task<IEnumerable<Matricula>> GetAllAsync();
        Task<IEnumerable<Matricula>> GetByAlunoIdAsync(int alunoId);
        Task<IEnumerable<Matricula>> GetByCursoIdAsync(int cursoId);
        Task<Matricula> CreateMatriculaAsync(Matricula matricula);
        Task UpdateMatriculaAsync(Matricula matricula);
        Task DeleteMatriculaAsync(int id);
    }
}