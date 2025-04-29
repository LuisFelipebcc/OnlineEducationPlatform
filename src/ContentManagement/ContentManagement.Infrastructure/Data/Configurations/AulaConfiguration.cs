using ContentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentManagement.Infrastructure.Data.Configurations
{
    public class AulaConfiguration : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Descricao)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(a => a.Conteudo)
                .IsRequired();

            builder.Property(a => a.Ordem)
                .IsRequired();

            builder.Property(a => a.CursoId)
                .IsRequired();

            builder.Property(a => a.DataCriacao)
                .IsRequired();

            builder.OwnsOne(a => a.ConteudoProgramatico, cp =>
            {
                cp.Property(c => c.Descricao)
                    .IsRequired()
                    .HasMaxLength(1000);

                cp.Property(c => c.Objetivos)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );

                cp.Property(c => c.PreRequisitos)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });
        }
    }
}