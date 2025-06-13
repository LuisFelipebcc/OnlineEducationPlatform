using IdentityService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace IdentityService.Domain.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin?> GetByIdAsync(Guid id); // O ID é o mesmo do User
        Task AddAsync(Admin admin);
        Task UpdateAsync(Admin admin);
        Task RemoveAsync(Admin admin);
        // Outros métodos específicos para consulta ou manipulação de Admins podem ser adicionados aqui.
    }
}