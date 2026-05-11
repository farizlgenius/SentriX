using System;
using Microsoft.EntityFrameworkCore;
using Operator.Infrastructure.Persistences.Entities;
using SharedKernel.Domain;

namespace Operator.Infrastructure.Persistences;

public sealed class OperatorDbContext(DbContextOptions<OperatorDbContext> options) : DbContext(options)
{
      public const string Schema = "operator";
      public DbSet<Operators> operators { get; set; }
      public DbSet<OperatorLocation> operator_locations {get; set;}

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            // ⭐ Module schema
            modelBuilder.HasDefaultSchema(Schema);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                  if (typeof(BaseEntity).IsAssignableFrom(entity.ClrType))
                  {
                        modelBuilder.Entity(entity.ClrType)
                            .Property(nameof(BaseEntity.created_at))
                            .HasDefaultValueSql("NOW()");

                        modelBuilder.Entity(entity.ClrType)
                            .Property(nameof(BaseEntity.updated_at))
                            .HasDefaultValueSql("NOW()");
                  }
            }

            modelBuilder.Entity<Operators>()
            .HasMany(o => o.operator_locations)
            .WithOne(x => x.operators)
            .HasForeignKey(ol => ol.operator_id)
            .OnDelete(DeleteBehavior.Cascade);

            // ⭐ Seed Data
            modelBuilder.Entity<Operators>().HasData(
                 new Operators
                 {
                       id = 1,
                       username = "admin",
                       password = "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=",
                       title = "Administrator",
                       firstname = "Administrator",
                       middlename = "",
                       lastname = "",
                       gender = "M",
                       email = "admin@sentrix.com",
                       mobile = "1234567890",
                       role_id = 1,
                 }
            );

            modelBuilder.Entity<OperatorLocation>().HasData(
                 new OperatorLocation
                 {
                       id = 1,
                       operator_id = 1,
                       location_id = 1
                 }
            );

      }
}
