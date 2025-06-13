using ContentManagement.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentManagement.Infrastructure.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.Duration)
            .IsRequired();

        builder.Property(c => c.Level)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        builder.OwnsOne(c => c.CourseContent, cp =>
        {
            cp.Property(p => p.Introduction)
                .IsRequired()
                .HasMaxLength(2000);

            cp.Property(p => p.Objectives)
                .IsRequired()
                .HasMaxLength(2000);

            cp.Property(p => p.Prerequisites)
                .IsRequired()
                .HasMaxLength(2000);

            cp.Property(p => p.TargetAudience)
                .IsRequired()
                .HasMaxLength(2000);

            cp.Property(p => p.Methodology)
                .IsRequired()
                .HasMaxLength(2000);

            cp.Property(p => p.Evaluation)
                .IsRequired()
                .HasMaxLength(2000);

            cp.Property(p => p.Certification)
                .IsRequired()
                .HasMaxLength(2000);

            cp.Property(p => p.References)
                .IsRequired()
                .HasMaxLength(2000);
        });

        builder.HasMany(c => c.Lessons)
            .WithOne()
            .HasForeignKey(l => l.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.Title);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.CreatedAt);
        builder.HasIndex(c => new { c.Title, c.Status }).IsUnique();

        builder.Property<bool>("IsDeleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(c => EF.Property<bool>(c, "IsDeleted") == false);
    }
}