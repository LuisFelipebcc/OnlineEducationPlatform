using ContentManagement.Domain.Aggregates;
using ContentManagement.Domain.Entities;
using ContentManagement.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Context;

public class ContentManagementDbContext : DbContext
{
    public ContentManagementDbContext(DbContextOptions<ContentManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Aula> Aulas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CursoConfiguration());
        modelBuilder.ApplyConfiguration(new AulaConfiguration());
    }
}