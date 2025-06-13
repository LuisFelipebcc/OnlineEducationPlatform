using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Data
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.Id).HasColumnName("UserId");
                entity.Property(e => e.UserName).HasColumnName("Username");
                entity.Property(e => e.NormalizedUserName).HasColumnName("NormalizedUsername");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.NormalizedEmail).HasColumnName("NormalizedEmail");
                entity.Property(e => e.EmailConfirmed).HasColumnName("EmailConfirmed");
                entity.Property(e => e.PasswordHash).HasColumnName("PasswordHash");
                entity.Property(e => e.SecurityStamp).HasColumnName("SecurityStamp");
                entity.Property(e => e.ConcurrencyStamp).HasColumnName("ConcurrencyStamp");
                entity.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");
                entity.Property(e => e.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
                entity.Property(e => e.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
                entity.Property(e => e.LockoutEnd).HasColumnName("LockoutEnd");
                entity.Property(e => e.LockoutEnabled).HasColumnName("LockoutEnabled");
                entity.Property(e => e.AccessFailedCount).HasColumnName("AccessFailedCount");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("Roles");
                entity.Property(e => e.Id).HasColumnName("RoleId");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.NormalizedName).HasColumnName("NormalizedName");
                entity.Property(e => e.ConcurrencyStamp).HasColumnName("ConcurrencyStamp");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
                entity.Property(e => e.Id).HasColumnName("ClaimId");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.ClaimType).HasColumnName("ClaimType");
                entity.Property(e => e.ClaimValue).HasColumnName("ClaimValue");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.LoginProvider).HasColumnName("LoginProvider");
                entity.Property(e => e.ProviderKey).HasColumnName("ProviderKey");
                entity.Property(e => e.ProviderDisplayName).HasColumnName("ProviderDisplayName");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.LoginProvider).HasColumnName("LoginProvider");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Value).HasColumnName("Value");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
                entity.Property(e => e.Id).HasColumnName("ClaimId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
                entity.Property(e => e.ClaimType).HasColumnName("ClaimType");
                entity.Property(e => e.ClaimValue).HasColumnName("ClaimValue");
            });
        }
    }
}
