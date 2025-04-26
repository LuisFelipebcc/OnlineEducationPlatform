using ContentManagement.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentManagement.Infrastructure.Configurations;

public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.Preco)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.Ativo)
            .IsRequired();

        builder.Property(c => c.DataCriacao)
            .IsRequired();

        builder.HasMany(c => c.Aulas)
            .WithOne()
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}