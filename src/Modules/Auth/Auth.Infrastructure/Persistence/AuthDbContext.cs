using System;
using Auth.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Auth.Infrastructure.Persistence;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
      public const string Schema = "auth";
      public DbSet<ApiKey> ApiKeys { get; set; }
      public DbSet<RefreshTokenAudit> RefreshTokenAudits { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            // ⭐ Module schema
            modelBuilder.HasDefaultSchema(Schema);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                  if (typeof(BaseEntity).IsAssignableFrom(entity.ClrType))
                  {
                        modelBuilder.Entity(entity.ClrType)
                            .Property(nameof(BaseEntity.created_at))
                            .HasDefaultValueSql("NOW()");

                        modelBuilder.Entity(entity.ClrType)
                            .Property(nameof(BaseEntity.updated_at))
                            .HasDefaultValueSql("NOW()");
                  }
            }

            modelBuilder.Entity<ApiKey>()
            .Property(nameof(ApiKey.expired_at))
            .HasDefaultValueSql("NOW()");
      }
}
