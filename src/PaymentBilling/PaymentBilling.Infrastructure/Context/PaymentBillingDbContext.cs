using Microsoft.EntityFrameworkCore;
using PaymentBilling.Domain.Entities;
using StudentManagement.Domain.Entities;

namespace PaymentBilling.Infrastructure.Context;

public class PaymentBillingDbContext : DbContext
{
    public PaymentBillingDbContext(DbContextOptions<PaymentBillingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            // Nome da tabela
            entity.ToTable("Pagamentos");

            // Chave primária
            entity.HasKey(e => e.Id);

            // Configuração de propriedades
            entity.Property(e => e.Valor)
                .HasPrecision(18, 2);

            entity.Property(e => e.Status)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(e => e.MetodoPagamento)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(e => e.NumeroTransacao)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Observacoes)
                .HasMaxLength(500);

            // Relacionamentos
            entity.HasOne<Student>()
                .WithMany()
                .HasForeignKey(e => e.AlunoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Matricula>()
                .WithMany()
                .HasForeignKey(e => e.MatriculaId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}