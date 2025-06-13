using ContentManagement.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentManagement.Infrastructure.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.OwnsOne(c => c.CourseContent, cp =>
            {
                cp.Property(c => c.Description)
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

            builder.HasMany(c => c.Lessons)
                .WithOne()
                .HasForeignKey(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}