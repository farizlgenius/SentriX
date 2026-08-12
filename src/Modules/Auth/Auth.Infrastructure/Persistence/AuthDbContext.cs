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

            var isSqlServer = Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer";
            var isPostgres = Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL";

            string utcNowSql;
            string guidSql;

            if (isSqlServer)
            {
                  utcNowSql = "GETUTCDATE()";
                  guidSql = "NEWSEQUENTIALID()"; // or NEWID()
            }
            else if (isPostgres)
            {
                  utcNowSql = "NOW() AT TIME ZONE 'UTC'";
                  guidSql = "gen_random_uuid()";
            }
            else
            {
                  throw new Exception("Unsupported database provider");
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {

                  if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                  {
                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(BaseEntity.created_at))
                              .HasDefaultValueSql(utcNowSql)
                              .ValueGeneratedOnAdd();

                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(BaseEntity.updated_at))
                              .HasDefaultValueSql(utcNowSql)
                              .ValueGeneratedOnAdd();

                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(ApiKey.expired_at))
                              .HasDefaultValueSql(utcNowSql)
                              .ValueGeneratedOnAdd();

                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(BaseEntity.guid))
                              .HasDefaultValueSql(guidSql)
                              .ValueGeneratedOnAdd();
                  }

            }

      }
}
