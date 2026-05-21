using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using Time.Infrastructure.Persistences.Entities;

namespace Time.Infrastructure.Persistences;

public sealed class TimeDbContext(DbContextOptions<TimeDbContext> options) : DbContext(options)
{
      public const string Schema = "time";
      public DbSet<Holiday> Holidays { get; set; }
      public DbSet<Timezone> Timezones {get; set;}
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

            modelBuilder.Entity<Timezone>()
            .HasMany(x => x.intervals)
            .WithOne(x => x.timezone)
            .HasForeignKey(x => x.timezone_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Interval>()
            .HasOne(x => x.days)
            .WithOne(x => x.interval)
            .HasForeignKey<DayInWeek>(x => x.interval_id)
            .OnDelete(DeleteBehavior.Cascade);

           

      }
}