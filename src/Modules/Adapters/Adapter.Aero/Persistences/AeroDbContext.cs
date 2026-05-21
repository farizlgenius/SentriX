
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Adapter.Aero.Persistences;

public sealed class AeroDbContext(DbContextOptions<AeroDbContext> options) : DbContext(options)
{
      public const string Schema = "aero";
      public DbSet<ScpDeviceSpecification> ScpDeviceSpecifications { get; set; }
      public DbSet<AccessDatabaseSpecification> AccessDatabaseSpecifications { get; set; }
      public DbSet<ElevatorAccessLevelSpecification> ElevatorAccessLevelSpecifications { get; set; }
      public DbSet<RelayMode> RelayModes {get; set;}
      public DbSet<TimezoneMode> TimezoneModes {get; set;}
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

            // Below is used for define relation betweeneach database 
            modelBuilder.Entity<Aeros>()
            .HasMany(x => x.driver_configurations)
            .WithOne(x => x.aero)
            .HasForeignKey(x => x.aero_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Aeros>()
            .HasMany(x => x.sio_panel_configurations)
            .WithOne(x => x.aero)
            .HasForeignKey(x => x.aero_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Aeros>()
            .HasMany(x => x.input_point_specifications)
            .WithOne(x => x.aero)
            .HasForeignKey(x => x.aero_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Aeros>()
            .HasMany(x => x.output_point_specifications)
            .WithOne(x => x.aero)
            .HasForeignKey(x => x.aero_id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Aeros>()
            .HasMany(x => x.control_point_configurations)
            .WithOne(x => x.aero)
            .HasForeignKey(x => x.aero_id)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ScpDeviceSpecification>()
            .HasData(
                new ScpDeviceSpecification
                {
                      id = 1,
                      scp_id = 0,
                      mac = string.Empty,
                      n_msp1_port = 3,
                      n_transcations = 60000,
                      n_sio = 33,
                      n_mp = 615,
                      n_cp = 388,
                      n_acr = 64,
                      n_alvl = 32000,
                      n_trgr = 1024,
                      n_proc = 1024,
                      gmt_offset = -25200,
                      n_dst_id = 0,
                      n_tz = 255,
                      n_hol = 255,
                      n_mpg = 128,
                      n_tran_limit = 60000,
                      n_oper_mode = 0,
                      oper_type = 1,
                      n_language = 0
                }
            );

            modelBuilder.Entity<AccessDatabaseSpecification>()
            .HasData(
                new AccessDatabaseSpecification
                {
                      id = 1,
                      scp_id = 0,
                      mac = string.Empty,
                      n_card = 1000,
                      n_alvl = 8,
                      n_pin_digits = 324,
                      b_issue_code = 1,
                      b_apb_location = 1,
                      b_act_date = 2,
                      b_deact_date = 2,
                      b_vacation_date = 1,
                      b_upgrade_date = 1,
                      b_user_level = 7,
                      b_use_limit = 1,
                      b_support_time_apb = 1,
                      n_tz = 64,
                      b_asset_group = 0,
                      n_host_response_timeout = 5,
                      n_alvl_use4arg = 0,
                      n_escort_timeout = 15,
                      n_multi_card_timeout = 15,

                }

            );

            modelBuilder.Entity<RelayMode>()
            .HasData(
                  new RelayMode(1,"Normal / No Change",0),
                  new RelayMode(2,"Inverted / No Change",1),
                  new RelayMode(3,"Normal / Inactive",16),
                  new RelayMode(4,"Inverted / Inactive",17),
                  new RelayMode(5,"Normal / Active",32),
                  new RelayMode(6,"Inverted / Active",33)
            );

            modelBuilder.Entity<TimezoneMode>()
            .HasData(
                  new TimezoneMode(1,"Off",0,"The time zone is always inactive, regardless of the time zone intervals specified or the holidays in effect."),
                  new TimezoneMode(2,"On",1,"The time zone is always active, regardless of the time zone intervals specified or the holidays in effect."),
                  new TimezoneMode(3,"Scan",2,"The Time Zone state is decided using either the Day Mask or the Holiday Mask. If the current day is specified as a Holiday, the state relies only on whether the Holiday Mask Flag for that Holiday is set (if today is Holiday 1, and the Holiday Mask sets flag H1, then the state is active. If today is Holiday 1, and the Holiday Mask does not have flag H1 set, then the state is inactive). Holidays override the standard accessibility rules. If the current day is not specified as a Holiday, the Time Zone is active or inactive depending on whether the current day/time falls within the Day Mask. If Day Mask is M-F, 8-5, the Time Zone is active during those times, and inactive on the weekend and outside working hours."),
                  new TimezoneMode(4,"One Time Event",3,"Scan time zone interval list and apply only if the date string in expTest matches the current date"),
                  new TimezoneMode(5,"Scan, Always Honor Day of Week (OR)",4,"This mode is similar to mode 2, but instead of only checking the Holiday Mask if it is a Holiday, and only checking the Day Mask if not, this mode checks both. If it is not a Holiday, this mode functions normally, only checking the Day Mask. If it is a Holiday, this mode performs a logical OR on the Holiday and Day Masks. If either or both are active, the Time Zone is active, otherwise if neither is active, the Time Zone is inactive."),
                  new TimezoneMode(6,"Scan, Always Honor Day of Week (AND) ",5,"This mode is similar to mode 4, but it performs a logical AND instead of a logical OR. If it is not a Holiday, this mode functions normally, only checking the Day Mask. If it is a Holiday, this mode is only active if BOTH the Day Mask and Holiday Mask are active. If either one is inactive, the entire Time Zone is inactive.")
            );

          

            modelBuilder.Entity<ElevatorAccessLevelSpecification>()
                .HasData(
                    new ElevatorAccessLevelSpecification
                    {
                          id = 1,
                          scp_id = 0,
                          max_ealvl = 256,
                          max_floors = 128
                    }
                );


      }
}
