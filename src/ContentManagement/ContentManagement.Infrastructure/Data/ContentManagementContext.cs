using ContentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Data
{
    public class ContentManagementContext : DbContext
    {
        public ContentManagementContext(DbContextOptions<ContentManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContentManagementContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}