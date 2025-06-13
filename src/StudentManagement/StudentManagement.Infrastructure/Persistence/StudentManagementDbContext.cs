using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.ValueObjects; // Para HistoricoAprendizado
using System.Text.Json; // Para serialização do HistoricoAprendizado

namespace StudentManagement.Infrastructure.Persistence
{
    public class StudentManagementDbContext : DbContext
    {
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        public StudentManagementDbContext(DbContextOptions<StudentManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aluno>(builder =>
            {
                builder.HasKey(a => a.Id);
                // O Id do Aluno é o mesmo do User no IdentityService,
                // não há um relacionamento de FK direto aqui, pois são BCs diferentes.
                // A consistência do ID é garantida pela aplicação ao criar o Aluno.
                builder.Property(a => a.NomeCompleto).IsRequired().HasMaxLength(200);
                builder.Property(a => a.Email).IsRequired().HasMaxLength(256);
                builder.HasIndex(a => a.Email).IsUnique(); // Email do aluno deve ser único neste contexto também

                // Relacionamento com Matriculas
                builder.HasMany(a => a.Matriculas)
                       .WithOne() // Se Matricula não tiver prop de navegação para Aluno
                       .HasForeignKey(m => m.AlunoId)
                       .OnDelete(DeleteBehavior.Cascade); // Se Aluno for deletado, suas matrículas também

                // Relacionamento com Certificados
                builder.HasMany(a => a.Certificados)
                       .WithOne() // Se Certificado não tiver prop de navegação para Aluno
                       .HasForeignKey(c => c.AlunoId)
                       .OnDelete(DeleteBehavior.Cascade); // Se Aluno for deletado, seus certificados também
            });

            modelBuilder.Entity<Matricula>(builder =>
            {
                builder.HasKey(m => m.Id);
                builder.Property(m => m.PrecoPago).HasColumnType("decimal(18,2)");

                // Configuração para o Value Object HistoricoAprendizado
                // Pode ser como Owned Type ou serializado para JSON
                builder.OwnsOne(m => m.HistoricoAprendizado, haBuilder =>
                {
                    haBuilder.Property(ha => ha.ProgressoAulas)
                        .HasConversion(
                            d => JsonSerializer.Serialize(d, (JsonSerializerOptions?)null),
                            s => JsonSerializer.Deserialize<Dictionary<Guid, DateTime?>>(s, (JsonSerializerOptions?)null) ?? new Dictionary<Guid, DateTime?>()
                        )
                        .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<IReadOnlyDictionary<Guid, DateTime?>>(
                            (c1, c2) => c1!.SequenceEqual(c2!),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                            c => (IReadOnlyDictionary<Guid, DateTime?>)c.ToDictionary(k => k.Key, v => v.Value)
                        )); // Comparador para dicionário
                });
            });

            modelBuilder.Entity<Certificado>(builder =>
            {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.NomeCurso).IsRequired().HasMaxLength(200);
                builder.Property(c => c.CodigoVerificacao).IsRequired().HasMaxLength(12);
                builder.HasIndex(c => c.CodigoVerificacao).IsUnique();
            });
        }
    }
}