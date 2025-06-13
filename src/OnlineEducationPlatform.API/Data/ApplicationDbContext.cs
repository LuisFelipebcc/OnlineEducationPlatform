using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.API.Models;

namespace OnlineEducationPlatform.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ApplicationUser
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Nome).HasMaxLength(100);
                entity.Property(e => e.Sobrenome).HasMaxLength(100);
                entity.Property(e => e.CPF).HasMaxLength(11);
                entity.Property(e => e.Endereco).HasMaxLength(200);
                entity.Property(e => e.Cidade).HasMaxLength(100);
                entity.Property(e => e.Estado).HasMaxLength(2);
                entity.Property(e => e.CEP).HasMaxLength(8);
                entity.Property(e => e.Telefone).HasMaxLength(20);
            });
        }
    }
}