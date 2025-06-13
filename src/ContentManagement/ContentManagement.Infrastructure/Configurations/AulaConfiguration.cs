using ContentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentManagement.Infrastructure.Configurations;

public class AulaConfiguration : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> builder)
    {
        builder.ToTable("Aula");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(a => a.Content)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(a => a.DuracaoEmMinutos)
            .IsRequired();

        builder.Property(a => a.Ordem)
            .IsRequired();

        builder.Property(a => a.CursoId)
            .IsRequired();

        builder.HasOne<Aula>()
            .WithMany(c => c.Aulas)
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.Id);
        builder.HasIndex(a => a.Ordem);
        builder.HasIndex(a => a.CursoId);
        builder.HasIndex(a => new { a.CourseId, a.Order }).IsUnique();

        builder.Property<bool>("IsDeleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(a => EF.Property<bool>(a, "IsDeleted") == false);
    }
}