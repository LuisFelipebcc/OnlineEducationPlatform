using ContentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ContentManagement.Infrastructure.Persistence
{
    public class ContentManagementDbContext : DbContext
    {
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }

        public ContentManagementDbContext(DbContextOptions<ContentManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas as configurações de entidade definidas neste assembly
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Ou configurar manualmente:

            modelBuilder.Entity<Curso>(builder =>
            {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.Titulo).IsRequired().HasMaxLength(200);
                builder.Property(c => c.Descricao).HasMaxLength(1000);
                builder.Property(c => c.Preco).HasColumnType("decimal(18,2)");

                // Configuração para o Value Object ConteudoProgramatico como Owned Type
                builder.OwnsOne(c => c.ConteudoProgramatico, cpBuilder =>
                {
                    cpBuilder.Property(cp => cp.Topicos).HasConversion(
                        v => string.Join(';', v),
                        v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList());
                    cpBuilder.Property(cp => cp.ObjetivosAprendizagem).HasConversion(
                        v => string.Join(';', v),
                        v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList());
                });

                // Relacionamento com Aulas (Curso tem muitas Aulas)
                builder.HasMany(c => c.Aulas)
                       .WithOne() // Se Aula não tiver uma propriedade de navegação para Curso
                       .HasForeignKey(a => a.CursoId)
                       .OnDelete(DeleteBehavior.Cascade); // Excluir aulas se o curso for excluído
            });

            modelBuilder.Entity<Aula>(builder =>
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.Titulo).IsRequired().HasMaxLength(200);
                builder.Property(a => a.Descricao).HasMaxLength(1000);
                // A relação com Curso já está definida acima.
            });
        }
    }
}