using IdentityService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace IdentityService.Domain.Repositories
{
    public interface IAlunoRepository
    {
        Task<Aluno?> GetByIdAsync(Guid id); // O ID é o mesmo do User
        Task AddAsync(Aluno aluno);
        Task UpdateAsync(Aluno aluno);
        Task RemoveAsync(Aluno aluno);
        // Outros métodos específicos para consulta ou manipulação de Alunos podem ser adicionados aqui.
    }
}