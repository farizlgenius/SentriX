using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Adapter.Amico.Persistences;

public class AmicoDbContext(DbContextOptions<AmicoDbContext> options) : DbContext(options)
{
      public const string Schema = "amico";
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


      }
}
