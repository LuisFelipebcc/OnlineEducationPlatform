using ContentManagement.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Data
{
    public class ContentManagementContext : DbContext
    {
        public ContentManagementContext(DbContextOptions<ContentManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContentManagementContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}