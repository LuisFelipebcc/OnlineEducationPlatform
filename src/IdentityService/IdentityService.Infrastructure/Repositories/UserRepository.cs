using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            // Ou _context.Entry(user).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(User user)
        {
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }

        // Se GetByRefreshTokenAsync for necessário, implementar aqui.
    }
}