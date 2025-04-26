using ContentManagement.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentManagement.Infrastructure.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Tipo)
            .IsRequired();

        builder.Property(u => u.Ativo)
            .IsRequired();

        builder.Property(u => u.DataCriacao)
            .IsRequired();

        // Relacionamento com Persona
        builder.HasOne(u => u.Persona)
            .WithOne(p => p.Usuario)
            .HasForeignKey<Persona>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}