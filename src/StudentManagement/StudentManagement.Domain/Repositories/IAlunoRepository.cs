using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Repositories
{
    public interface IAlunoRepository
    {
        Task<Aluno?> GetByIdAsync(Guid id); // O ID é o mesmo do User no IdentityService
        Task<IEnumerable<Aluno>> GetAllAsync();
        // Poderiam existir métodos para buscar alunos por email, nome, etc.
        // Ex: Task<Aluno?> GetByEmailAsync(string email);

        Task AddAsync(Aluno aluno);
        Task UpdateAsync(Aluno aluno);
        Task RemoveAsync(Aluno aluno); // Geralmente, a remoção de um aluno pode ser complexa devido a dependências.
    }
}