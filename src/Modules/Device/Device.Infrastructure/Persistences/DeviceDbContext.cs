using System;
using Device.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Device.Infrastructure.Persistences;

public sealed class DeviceDbContext(DbContextOptions<DeviceDbContext> options) : DbContext(options)
{
      public const string Schema = "device";
      public DbSet<Devices> Devices { get; set; }
      public DbSet<Module> Modules {get; set;}

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

            modelBuilder.Entity<Devices>()
            .HasMany(x => x.modules)
            .WithOne(x => x.devices)
            .HasForeignKey(x => x.device_id)
            .OnDelete(DeleteBehavior.Cascade);
      }

}
