using Group.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Domain;

namespace Group.Infrastructure.Persistences;

public sealed class GroupDbContext(DbContextOptions<GroupDbContext> options) : DbContext(options)
{
      public const string Schema = "group";
      public DbSet<Persistences.Entities.Groups> Groups { get; set; }
      public DbSet<Persistences.Entities.GroupDoor> GroupDoors { get; set; }

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

            modelBuilder.Entity<Persistences.Entities.Groups>()
            .HasMany(x => x.group_doors)
            .WithOne(x => x.groups)
            .HasForeignKey(x => x.group_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Persistences.Entities.Groups>()
            .HasData(
                  new Persistences.Entities.Groups
                  {
                        id=1,
                        name="Default",
                        component_id=1,
                        location_id=-1,
                        is_default=true,
                        is_active=true
                  },
                  new Persistences.Entities.Groups
                  {
                        id=2,
                        name="Always",
                        component_id=1,
                        location_id=0,
                        is_default=true,
                        is_active=true
                  },
                   new Persistences.Entities.Groups
                  {
                        id=3,
                        name="Never",
                        component_id=2,
                        location_id=0,
                        is_default=true,
                        is_active=true
                  }
            );


      }


}