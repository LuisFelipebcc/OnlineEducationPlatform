using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Interfaces
{
    public interface IMatriculaRepository
    {
        Task<Matricula> GetByIdAsync(int id);
        Task<IEnumerable<Matricula>> GetAllAsync();
        Task<IEnumerable<Matricula>> GetByAlunoIdAsync(int alunoId);
        Task<IEnumerable<Matricula>> GetByCursoIdAsync(int cursoId);
        Task<Matricula> AddAsync(Matricula matricula);
        Task UpdateAsync(Matricula matricula);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int alunoId, int cursoId);
        Task<IEnumerable<Matricula>> GetMatriculasAtivasAsync();
        Task<IEnumerable<Matricula>> GetMatriculasConcluidasAsync();
    }
}