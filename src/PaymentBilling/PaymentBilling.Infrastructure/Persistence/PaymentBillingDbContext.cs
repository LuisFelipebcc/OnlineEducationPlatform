using Microsoft.EntityFrameworkCore;
using PaymentBilling.Domain.Entities;

namespace PaymentBilling.Infrastructure.Persistence
{
    public class PaymentBillingDbContext : DbContext
    {
        public DbSet<Pagamento> Pagamentos { get; set; }

        public PaymentBillingDbContext(DbContextOptions<PaymentBillingDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pagamento>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Valor).HasColumnType("decimal(18,2)");
                builder.Property(p => p.Status).IsRequired();

                // Configuração para o Value Object DadosCartao como Owned Type
                // Isso significa que as propriedades de DadosCartao serão colunas na tabela Pagamentos.
                builder.OwnsOne(p => p.DadosCartaoUtilizado, dcBuilder =>
                {
                    // O EF Core por padrão nomeia as colunas como NomeDaPropriedade_NomeDaPropriedadeDoVO
                    // Ex: DadosCartaoUtilizado_NumeroCartaoMascarado
                    // Você pode customizar os nomes das colunas se desejar com .Property(vo => vo.Prop).HasColumnName("NomeColuna")
                    dcBuilder.Property(dc => dc.NumeroCartaoMascarado).HasMaxLength(20); // Ajuste o tamanho conforme necessário
                    dcBuilder.Property(dc => dc.NomeTitular).HasMaxLength(100);
                    dcBuilder.Property(dc => dc.DataValidade).HasMaxLength(5);
                });

                builder.HasIndex(p => p.AlunoId);
                builder.HasIndex(p => p.ReferenciaPedidoId);
                builder.HasIndex(p => p.GatewayPagamentoId);
            });
        }
    }
}