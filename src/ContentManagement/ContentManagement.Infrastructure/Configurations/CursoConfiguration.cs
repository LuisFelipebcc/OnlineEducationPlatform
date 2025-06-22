using ContentManagement.Domain.Entities;
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

        builder.Property(c => c.InstrutorId)
            .IsRequired();

        builder.Property(c => c.DataPublicacao)
            .IsRequired();

        builder.Property(c => c.Preco)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.OwnsOne(c => c.ConteudoProgramatico, cp =>
        {
            cp.Property(p => p.Topicos)
                .HasConversion(
                    v => string.Join(";", v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries))
                .HasColumnName("Topicos")
                .IsRequired();

            cp.Property(p => p.ObjetivosAprendizagem)
                .HasConversion(
                    v => string.Join(";", v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries))
                .HasColumnName("ObjetivosAprendizagem")
                .IsRequired();
        });

        builder.Ignore(c => c.Aulas);

        builder.HasMany<Aula>("_aulas")
            .WithOne()
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.Titulo);
        builder.HasIndex(c => c.InstrutorId);
        builder.HasIndex(c => c.DataPublicacao);
    }
}