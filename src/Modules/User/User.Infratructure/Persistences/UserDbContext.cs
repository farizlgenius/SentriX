using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using User.Infratructure.Persistences.Entities;

namespace User.Infratructure.Persistences;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
      public const string Schema = "device";

      public DbSet<Users> Users { get; set; }
      public DbSet<Company> Companies {get; set;}
      public DbSet<Department> Departments {get; set;}
      public DbSet<Position> Positions {get; set;}
      public DbSet<Credential> Credentials {get; set;}

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            // ⭐ Module schema
            modelBuilder.HasDefaultSchema(Schema);

            // Make default datetime now
            var isSqlServer = Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer";
            var isPostgres = Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL";

            string utcNowSql;

            if (isSqlServer)
                  utcNowSql = "GETUTCDATE()";
            else if (isPostgres)
                  utcNowSql = "NOW() AT TIME ZONE 'UTC'";
            else
                  throw new Exception("Unsupported database provider");


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
                  }
            }

      }
}
