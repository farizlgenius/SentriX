using System;
using Adapter.Amico.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Adapter.Amico.Persistences;

public class AmicoDbContext(DbContextOptions<AmicoDbContext> options) : DbContext(options)
{
      public const string Schema = "amico";

      public DbSet<Amicos> Amicos {get; set;}
      public DbSet<Persistences.Entities.TimeSpan> TimeSpans { get; set; }
      public DbSet<Group> Groups { get; set; }
      public DbSet<AccessRule> AccessRules { get; set; }
      public DbSet<Persistences.Entities.TimeZone> TimeZone { get; set; }
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
                  if (typeof(AmicoDbEntity).IsAssignableFrom(entityType.ClrType))
                  {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property(nameof(AmicoDbEntity.created_at))
                            .HasDefaultValueSql(utcNowSql)
                            .ValueGeneratedOnAdd();

                        modelBuilder.Entity(entityType.ClrType)
                            .Property(nameof(AmicoDbEntity.updated_at))
                            .HasDefaultValueSql(utcNowSql)
                            .ValueGeneratedOnAdd();

                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(AmicoDbEntity.guid))
                              .HasDefaultValueSql("gen_random_uuid()")
                              .ValueGeneratedOnAdd();

                        
                  }

                  if (typeof(BaseSlot).IsAssignableFrom(entityType.ClrType))
                  {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property(nameof(BaseSlot.created_at))
                            .HasDefaultValueSql(utcNowSql)
                            .ValueGeneratedOnAdd();

                        modelBuilder.Entity(entityType.ClrType)
                            .Property(nameof(BaseSlot.updated_at))
                            .HasDefaultValueSql(utcNowSql)
                            .ValueGeneratedOnAdd();

                        
                  }
            }

            modelBuilder.Entity<Group>()
            .HasOne(x => x.access_rule)
            .WithOne(x => x.group)
            .HasForeignKey<Group>(x => x.access_rule_guid)
            .HasPrincipalKey<AccessRule>(x => x.guid);


      }
}
