using ContentManagement.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentManagement.Infrastructure.Configurations;

public class MatriculaConfiguration : IEntityTypeConfiguration<Matricula>
{
    public void Configure(EntityTypeBuilder<Matricula> builder)
    {
        builder.ToTable("Matriculas");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.DataMatricula)
            .IsRequired();

        builder.Property(m => m.Ativa)
            .IsRequired();

        // Relacionamento com Aluno
        builder.HasOne(m => m.Aluno)
            .WithMany(a => a.Matriculas)
            .HasForeignKey(m => m.Aluno.Id)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Curso
        builder.HasOne(m => m.Curso)
            .WithMany()
            .HasForeignKey(m => m.Curso.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}