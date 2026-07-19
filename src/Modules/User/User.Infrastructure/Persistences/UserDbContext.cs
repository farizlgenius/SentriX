using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using User.Infrastructure.Persistences.Entities;

namespace User.Infrastructure.Persistences;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
      public const string Schema = "user";

      public DbSet<Users> Users { get; set; }
      public DbSet<Company> Companies {get; set;}
      public DbSet<Department> Departments {get; set;}
      public DbSet<Position> Positions {get; set;}
      public DbSet<UserAdditional> UserAdditionals {get; set;}
      public DbSet<UserGroup> UserGroups {get; set;}
      public DbSet<UserFlag> UserFlags {get; set;}
      public DbSet<Card> Cards { get; set; }
      public DbSet<LicensePlate> LicensePlates { get; set; }
      public DbSet<Pin> Pins { get; set; }
      public DbSet<QrCode> QrCodes { get; set; }
      public DbSet<Face> Faces { get; set; }

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


            modelBuilder.Entity<Users>()
            .HasMany(x => x.additionals)
            .WithOne(x => x.user)
            .HasForeignKey(x => x.user_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasMany(x => x.user_groups)
            .WithOne(x => x.user)
            .HasForeignKey(x => x.user_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasOne(x => x.company)
            .WithMany(x => x.users)
            .HasForeignKey(x => x.company_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasOne(x => x.department)
            .WithMany(x => x.users)
            .HasForeignKey(x => x.department_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasOne(x => x.position)
            .WithMany(x => x.users)
            .HasForeignKey(x => x.position_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Company>()
            .HasMany(x => x.departments)
            .WithOne(x => x.company)
            .HasForeignKey(x => x.company_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
            .HasMany(x => x.positions)
            .WithOne(x => x.department)
            .HasForeignKey(x => x.department_guid)
            .HasPrincipalKey(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasOne(x => x.card)
            .WithOne(x => x.user)
            .HasForeignKey<Card>(x => x.user_guid)
            .HasPrincipalKey<Users>(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasOne(x => x.license_plate)
            .WithOne(x => x.user)
            .HasForeignKey<Card>(x => x.user_guid)
            .HasPrincipalKey<Users>(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasOne(x => x.qr_code)
            .WithOne(x => x.user)
            .HasForeignKey<Card>(x => x.user_guid)
            .HasPrincipalKey<Users>(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasOne(x => x.face)
            .WithOne(x => x.user)
            .HasForeignKey<Card>(x => x.user_guid)
            .HasPrincipalKey<Users>(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
            .HasOne(x => x.pin)
            .WithOne(x => x.user)
            .HasForeignKey<Card>(x => x.user_guid)
            .HasPrincipalKey<Users>(x => x.guid)
            .OnDelete(DeleteBehavior.Cascade);

            

            modelBuilder.Entity<UserFlag>()
            .HasData(
                  new UserFlag
                  {
                        id=1,
                        label="Active",
                        value=0x01,
                        description="Active cardholder record"

                  },
                  new UserFlag
                  {
                        id=2,
                        label="One free APB",
                        value=0x02,
                        description="Allow one free anti-passback pass"

                  },
                  new UserFlag
                  {
                        id=3,
                        label="APB Exempt",
                        value=0x04,
                        description="Anti-passback exempt"

                  },
                   new UserFlag
                  {
                        id=4,
                        label="ADA",
                        value=0x08,
                        description="Use timing parameters for the disabled (ADA)"

                  },
                  new UserFlag
                  {
                        id=5,
                        label="ADA",
                        value=0x08,
                        description="Use timing parameters for the disabled (ADA)"

                  },
                  new UserFlag
                  {
                        id=6,
                        label="PIN Exempt",
                        value=0x10,
                        description="PIN Exempt for 'Card & PIN' ACR mode"

                  },
                  new UserFlag
                  {
                        id=7,
                        label="No Change APB Location",
                        value=0x20,
                        description="Do not change apb_loc"

                  },
                  new UserFlag
                  {
                        id=8,
                        label="No Change Use Limit",
                        value=0x40,
                        description="Do not alter either the 'original' or the 'current' use count values"

                  },
                  new UserFlag
                  {
                        id=9,
                        label="No Change Current",
                        value=0x80,
                        description="Do not alter the 'current' use count but change the original use limit stored in the cardholder database"

                  }
            );

      }
}
