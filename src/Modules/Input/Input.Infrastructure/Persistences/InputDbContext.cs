using Input.Domain.Entities;
using Input.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Input.Infrastructure.Persistences;

public sealed class InputDbContext(DbContextOptions<InputDbContext> options) : DbContext(options)
{
      public const string Schema = "input";
      public DbSet<Persistences.Entities.Inputs> Inputs { get; set; }
      public DbSet<Persistences.Entities.InputGroups> InputGroups {get; set;}
      public DbSet<Persistences.Entities.InputMode> InputModes {get; set;}

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

            modelBuilder.Entity<InputMode>()
            .HasData(
                  new InputMode
                  {
                        id=1,
                        label="Normally closed",
                        value=0,
                        description=""
                  },
                   new InputMode
                  {
                        id=2,
                        label="Normally open",
                        value=1,
                        description=""
                  },
                   new InputMode
                  {
                        id=3,
                        label="EOL: 1 kΩ normal, 2 kΩ active",
                        value=2,
                        description=""
                  },
                   new InputMode
                  {
                        id=4,
                        label="EOL: 2 kΩ normal, 1 kΩ active",
                        value=3,
                        description=""
                  }
            );

           

      }
}