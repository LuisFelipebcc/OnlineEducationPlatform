using ContentManagement.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentManagement.Infrastructure.Configurations;

public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
{
    public void Configure(EntityTypeBuilder<Persona> builder)
    {
        builder.ToTable("Personas");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Documento)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.DataCriacao)
            .IsRequired();

        // Configuração para herança
        builder.HasDiscriminator<string>("Tipo")
            .HasValue<Aluno>("Aluno")
            .HasValue<Administrador>("Administrador");
    }
}