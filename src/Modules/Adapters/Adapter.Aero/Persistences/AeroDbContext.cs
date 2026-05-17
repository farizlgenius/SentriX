using System;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Adapter.Aero.Persistences;

public sealed class AeroDbContext(DbContextOptions<AeroDbContext> options) : DbContext(options)
{
      public const string Schema = "aero";
      public DbSet<SystemLevelSpecification> SystemLevelSpecifications { get; set; }
      public DbSet<CreateChannel> CreateChannels { get; set; }
      public DbSet<ScpDeviceSpecification> ScpDeviceSpecifications { get; set; }
      public DbSet<AccessDatabaseSpecification> AccessDatabaseSpecifications { get; set; }
      public DbSet<WriterAudit> WriterAudits { get; set; }
      public DbSet<DriverConfiguration> DriverConfigurations { get; set; }
      public DbSet<SioPanelConfiguration> SioPanelConfigurations { get; set; }
      public DbSet<InputPointSpecification> InputPointSpecifications { get; set; }
      public DbSet<Aeros> Aeros {get; set;}
      public DbSet<OutputPointSpecification> OutputPointSpecifications {get; set;}
      public DbSet<ControlPointConfiguration> ControlPointConfigurations {get; set;}

      public DbSet<ElevatorAccessLevelSpecification> ElevatorAccessLevelSpecifications { get; set; }
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

            modelBuilder.Entity<SystemLevelSpecification>()
        .HasData(
            new SystemLevelSpecification
            {
                  id = 1,
                  n_ports = 1024,
                  n_scps = 1024,
                  n_timezones = 0,
                  n_holidays = 0,
                  b_direct_mode = 1,
                  debug_rq = 0,
                  n_debug_arg = 0,
            }
        );

            modelBuilder.Entity<CreateChannel>()
            .HasData(
                new CreateChannel
                {
                      id = 1,
                      n_channel_id = 1,
                      c_type = 7,
                      c_port = 0,
                      baudrate = 0,
                      timer_1 = 3000,
                      timer_2 = 0,
                      c_model_id = 0,
                      c_rts_mode = 0
                }
            );

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

            // modelBuilder.Entity<DriverConfiguration>()
            // .HasData(
            //     new DriverConfiguration
            //     {
            //           id = 1,
            //           scp_id = 0,
            //           mac = string.Empty,
            //           msp1_number = 0,
            //           port_number = 3,
            //           baudrate = -1,
            //           reply_time = 0,
            //           n_protocol = 0,
            //           n_dialect = 0
            //     }
            // );

            // modelBuilder.Entity<SioPanelConfiguration>()
            // .HasData(
            //     new SioPanelConfiguration
            //     {
            //           id = 1,
            //           scp_id = 0,
            //           mac = string.Empty,
            //           n_inputs = SioModelHelper.nInputByModel(SioModel.x1100),
            //           n_outputs = SioModelHelper.nOutputByModel(SioModel.x1100),
            //           n_readers = SioModelHelper.nReaderByModel(SioModel.x1100),
            //           model = (short)SioModel.x1100,
            //           enable = 1,
            //           port = 0,
            //           address = 0,
            //           emax = 3,
            //           flags = 0,
            //           n_sio_next_in = -1,
            //           n_sio_next_out = -1,
            //           n_sio_next_rdr = -1
            //     }
            // );

            // modelBuilder.Entity<InputPointSpecification>()
            // .HasData(
            //     new InputPointSpecification
            //     {
            //           id = 1,
            //           scp_id = 0,
            //           mac = string.Empty,
            //           sio_number = 0,
            //           input_number = 0,
            //           icvt_num = 0,
            //           debounce = 2,
            //           hold_time = 5
            //     }
            // );

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
