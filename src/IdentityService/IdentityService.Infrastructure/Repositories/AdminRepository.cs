using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IdentityDbContext _context;

        public AdminRepository(IdentityDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Admin?> GetByIdAsync(Guid id)
        {
            return await _context.Admins.FindAsync(id);
        }

        public async Task AddAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
        }

        public Task UpdateAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            // Ou _context.Entry(admin).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Admin admin)
        {
            _context.Admins.Remove(admin);
            return Task.CompletedTask;
        }
    }
}