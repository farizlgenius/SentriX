using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using Time.Infrastructure.Persistences.Entities;

namespace Time.Infrastructure.Persistences;

public sealed class TimeDbContext(DbContextOptions<TimeDbContext> options) : DbContext(options)
{
      public const string Schema = "time";
      public DbSet<Holiday> Holidays { get; set; }
      public DbSet<Entities.TimeZone> Timezones {get; set;}
      public DbSet<Interval> Intervals {get; set;}
      public DbSet<DayInWeek> DayInWeeks {get; set;}

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            // ⭐ Module schema
            modelBuilder.HasDefaultSchema(Schema);

            // Make default datetime now
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
            if (typeof(BaseDbEntity).IsAssignableFrom(entityType.ClrType))
            {
                  modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseDbEntity.created_at))
                        .HasDefaultValueSql(utcNowSql)
                        .ValueGeneratedOnAdd();

                  modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseDbEntity.updated_at))
                        .HasDefaultValueSql(utcNowSql)
                        .ValueGeneratedOnAdd();

                  modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseDbEntity.guid))
                        .HasDefaultValueSql(guidSql)
                        .ValueGeneratedOnAdd();
            }
            }

            modelBuilder.Entity<Entities.TimeZone>()
            .HasMany(x => x.intervals)
            .WithOne(x => x.timezone)
            .HasForeignKey(x => x.timezone_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Interval>()
            .HasOne(x => x.days)
            .WithOne(x => x.interval)
            .HasForeignKey<DayInWeek>(x => x.interval_guid)
            .HasPrincipalKey<Interval>(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Persistences.Entities.TimeZone>()
            .HasData(
                  new Persistences.Entities.TimeZone
                  {
                        id=1,
                        name="Always",
                        guid=new Guid("9b6e1f89-6f6e-4c5d-a0a5-c9d6f5d18e7b"),
                        component_id=1,
                        mode=1,
                        active=string.Empty,
                        deactive=string.Empty,
                        is_default = true
                  },
                  new Persistences.Entities.TimeZone
                  {
                        id=2,
                        name="Never",
                        component_id=2,
                        mode=0,
                        active=string.Empty,
                        deactive = string.Empty,
                        is_default = true
                  }
            );

            modelBuilder.Entity<Persistences.Entities.Interval>()
            .HasData(
                  new Persistences.Entities.Interval
                  {
                        id=1,
                        guid=new Guid("f2d4c8b3-91aa-4b4c-8e1d-73c1f9b2a6d4"),
                        component_id=1,
                        start="00:00",
                        end="23:00",
                        timezone_guid=new Guid("9b6e1f89-6f6e-4c5d-a0a5-c9d6f5d18e7b")
                  }
            );

            modelBuilder.Entity<Persistences.Entities.DayInWeek>()
            .HasData(
                  new Persistences.Entities.DayInWeek
                  {
                        id=1,
                        guid=new Guid("4e7a2d90-3b8f-4fd8-9c57-2a1e6b9d8f43"),
                        sunday=true,
                        monday=true,
                        tuesday=true,
                        wednesday=true,
                        thursday=true,
                        friday=true,
                        saturday=true,
                        interval_guid=new Guid("f2d4c8b3-91aa-4b4c-8e1d-73c1f9b2a6d4"),
                  }
            );

           

      }
}