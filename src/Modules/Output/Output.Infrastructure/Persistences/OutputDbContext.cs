using System;
using Microsoft.EntityFrameworkCore;
using Output.Infrastructure.Persistences.Entities;
using SharedKernel.Domain;

namespace Output.Infrastructure.Persistences;

public sealed class OutputDbContext(DbContextOptions<OutputDbContext> options) : DbContext(options)
{
      public const string Schema = "output";
      public DbSet<Outputs> Outputs { get; set; }
      public DbSet<OutputDriveMode> OutputDriveModes {get; set;}
      public DbSet<OutputOfflineMode> OutputOfflineModes {get; set;}
      public DbSet<OutputMode> OutputModes {get; set;}

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

            modelBuilder.Entity<OutputDriveMode>()
            .HasData(
                  new OutputDriveMode
                  {
                        id=1,
                        value=0,
                        label="Normal"
                  },
                  new OutputDriveMode
                  {
                        id=2,
                        value=1,
                        label="Inverted"
                  }
            );

            modelBuilder.Entity<OutputOfflineMode>()
            .HasData(
                  new OutputOfflineMode
                  {
                        id=1,
                        value=0,
                        label="No Change"
                  },
                  new OutputOfflineMode
                  {
                        id=2,
                        value=1,
                        label="Inactive"
                  },
                  new OutputOfflineMode
                  {
                        id=3,
                        value=2,
                        label="Active"
                  }
            );

            modelBuilder.Entity<OutputMode>()
            .HasData(
                  new OutputMode
                  {
                        id = 1,
                        value=0,
                        drive=0,
                        offline=0
                  },
                  new OutputMode
                  {
                        id = 2,
                        value=1,
                        drive=1,
                        offline=0
                  },
                  new OutputMode
                  {
                        id = 3,
                        value=16,
                        drive=0,
                        offline=1
                  },
                  new OutputMode
                  {
                        id = 4,
                        value=17,
                        drive=1,
                        offline=1
                  },
                  new OutputMode
                  {
                        id = 5,
                        value=32,
                        drive=0,
                        offline=2
                  },
                  new OutputMode
                  {
                        id = 6,
                        value=33,
                        drive=1,
                        offline=2
                  }
            );

           

      }
}
