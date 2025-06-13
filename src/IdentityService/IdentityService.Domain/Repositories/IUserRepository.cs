using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(User user); // Recebe a entidade User já com o PasswordHash
        Task UpdateAsync(User user);
        Task RemoveAsync(User user);
    }
}