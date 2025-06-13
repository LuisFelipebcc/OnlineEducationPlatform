using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
                builder.HasIndex(u => u.Email).IsUnique(); // Garantir que o email seja único
                builder.Property(u => u.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<Aluno>(builder =>
            {
                // Id é a Chave Primária e também Chave Estrangeira para User.Id
                builder.HasKey(a => a.Id);
                builder.Property(a => a.NomeCompleto).IsRequired().HasMaxLength(200);

                // Relacionamento 1 para 1 com User
                builder.HasOne<User>()
                       .WithOne() // Se User não tiver propriedade de navegação para Aluno
                       .HasForeignKey<Aluno>(a => a.Id)
                       .OnDelete(DeleteBehavior.Cascade); // Se User for deletado, Aluno também será.
            });

            modelBuilder.Entity<Admin>(builder =>
            {
                builder.HasKey(adm => adm.Id);
                builder.Property(adm => adm.NomeCompleto).IsRequired().HasMaxLength(200);
                builder.Property(adm => adm.Cargo).HasMaxLength(100);

                builder.HasOne<User>()
                       .WithOne() // Se User não tiver propriedade de navegação para Admin
                       .HasForeignKey<Admin>(adm => adm.Id)
                       .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}