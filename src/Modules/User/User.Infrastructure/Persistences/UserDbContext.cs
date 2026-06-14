using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using User.Infrastructure.Persistences.Entities;

namespace User.Infrastructure.Persistences;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
      public const string Schema = "user";

      public DbSet<Users> Users { get; set; }
      public DbSet<Company> Companies {get; set;}
      public DbSet<Department> Departments {get; set;}
      public DbSet<Position> Positions {get; set;}
      public DbSet<Credential> Credentials {get; set;}
      public DbSet<UserAdditional> UserAdditionals {get; set;}
      public DbSet<UserGroup> UserGroups {get; set;}

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


            modelBuilder.Entity<Company>()
            .HasMany(x => x.users)
            .WithOne(x => x.company)
            .HasForeignKey(x => x.company_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
            .HasMany(x => x.departments)
            .WithOne(x => x.company)
            .HasForeignKey(x => x.company_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
            .HasMany(x => x.users)
            .WithOne(x => x.department)
            .HasForeignKey(x => x.department_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
            .HasMany(x => x.positions)
            .WithOne(x => x.department)
            .HasForeignKey(x => x.department_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Position>()
            .HasMany(x => x.users)
            .WithOne(x => x.position)
            .HasForeignKey(x => x.position_id)
            .OnDelete(DeleteBehavior.Cascade);

      }
}
