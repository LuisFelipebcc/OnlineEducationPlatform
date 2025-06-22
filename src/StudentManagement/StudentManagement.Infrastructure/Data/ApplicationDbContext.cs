using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de Aluno
            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NomeCompleto).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);

                entity.HasMany(e => e.Matriculas)
                      .WithOne()
                      .HasForeignKey(m => m.AlunoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Certificados)
                      .WithOne()
                      .HasForeignKey(c => c.AlunoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração de Matricula
            modelBuilder.Entity<Matricula>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CursoId).IsRequired();
                entity.Property(e => e.DataMatricula).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.PrecoPago).HasPrecision(18, 2).IsRequired();
            });

            // Configuração de Certificado
            modelBuilder.Entity<Certificado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CursoId).IsRequired();
                entity.Property(e => e.NomeCurso).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DataEmissao).IsRequired();
            });
        }
    }
}