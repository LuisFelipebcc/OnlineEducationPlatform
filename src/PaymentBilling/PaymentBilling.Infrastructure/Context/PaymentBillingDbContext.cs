using Microsoft.EntityFrameworkCore;
using PaymentBilling.Domain.Entities;

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
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Method).HasConversion<string>();
        });
    }
}