using System;
using Microsoft.EntityFrameworkCore;
using Role.Infrastructure.Persistences.Entities;
using SharedKernel.Domain;

namespace Role.Infrastructure.Persistences;

public class RoleDbContext(DbContextOptions<RoleDbContext> options) : DbContext(options)
{
      public const string Schema = "role";
      public DbSet<Feature> features {get; set;}
      public DbSet<Permission> permissions {get; set;}
      public DbSet<Roles> roles {get; set;}
      public DbSet<RoleOperator> role_operators {get; set;}

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

            // Relation
            modelBuilder.Entity<Roles>()
            .HasMany(r => r.role_operators)
            .WithOne(p => p.role)
            .HasForeignKey(p => p.role_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Roles>()
            .HasMany(r => r.permissions)
            .WithOne(p => p.role)
            .HasForeignKey(p => p.role_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feature>()
            .HasMany(p => p.permissions)
            .WithOne(f => f.feature)
            .HasForeignKey(p => p.feature_id)
            .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Feature>().HasData(
                  new Feature { id = 1, name = "dashboard", },
                  new Feature { id = 2, name = "events", },
                  new Feature { id = 3, name = "location", },
                  new Feature { id = 4, name = "alert", },
                  new Feature { id = 5, name = "operator", },
                  new Feature { id = 6, name = "device", },
                  new Feature { id = 7, name = "control", },
                  new Feature { id = 8, name = "monitor", },
                  new Feature { id = 9, name = "monitorgroup", },
                  new Feature { id = 10, name = "acr", },
                  new Feature { id = 11, name = "user", },
                  new Feature { id = 12, name = "group", },
                  new Feature { id = 13, name = "area", },
                  new Feature { id = 14, name = "time", },
                  new Feature { id = 15, name = "trigger", },
                  new Feature { id = 16, name = "map", },
                  new Feature { id = 17, name = "report", },
                  new Feature { id = 18, name = "setting", },
                  new Feature { id = 19, name = "tools", }
            );

            modelBuilder.Entity<Roles>()
            .HasData(
              new Roles { id = 1, name = "Administrator", location_id = 1 }
            );

            modelBuilder.Entity<Permission>()
            .HasData(
              new Permission { id = 1, role_id = 1, feature_id = 1, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 2, role_id = 1, feature_id = 2, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 3, role_id = 1, feature_id = 3, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 4, role_id = 1, feature_id = 4, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 5, role_id = 1, feature_id = 5, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 6, role_id = 1, feature_id = 6, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 7, role_id = 1, feature_id = 7, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 8, role_id = 1, feature_id = 8, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 9, role_id = 1, feature_id = 9, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 10, role_id = 1, feature_id = 10, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 11, role_id = 1, feature_id = 11, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 12, role_id = 1, feature_id = 12, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 13, role_id = 1, feature_id = 13, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 14, role_id = 1, feature_id = 14, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 15, role_id = 1, feature_id = 15, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 16, role_id = 1, feature_id = 16, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 17, role_id = 1, feature_id = 17, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 18, role_id = 1, feature_id = 18, is_enabled = true, is_created = true, is_deleted = true, is_updated = true },
              new Permission { id = 19, role_id = 1, feature_id = 19, is_enabled = true, is_created = true, is_deleted = true, is_updated = true }
            );

      }
}
