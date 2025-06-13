using ContentManagement.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).HasPrecision(10, 2);
                entity.Property(e => e.Level).IsRequired().HasMaxLength(50);

                entity.OwnsOne(e => e.CourseContent, courseContent =>
                {
                    courseContent.Property(c => c.Introduction).IsRequired();
                    courseContent.Property(c => c.Objectives).IsRequired();
                    courseContent.Property(c => c.Prerequisites).IsRequired();
                    courseContent.Property(c => c.TargetAudience).IsRequired();
                    courseContent.Property(c => c.Methodology).IsRequired();
                    courseContent.Property(c => c.Evaluation).IsRequired();
                    courseContent.Property(c => c.Certification).IsRequired();
                    courseContent.Property(c => c.References).IsRequired();
                });
            });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}