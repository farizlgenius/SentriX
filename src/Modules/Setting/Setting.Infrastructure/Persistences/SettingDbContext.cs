using Microsoft.EntityFrameworkCore;
using Setting.Infrastructure.Persistences.Entities;
using SharedKernel.Domain;

namespace Setting.Infrastructure.Persistences;

public class SettingDbContext(DbContextOptions<SettingDbContext> options) : DbContext(options)
{
      public const string Schema = "setting";
      // public DbSet<CardFormat> CardFormats { get; set; }
      public DbSet<WeakPassword> WeakPasswords { get; set; }
      public DbSet<PasswordRule> PasswordRules { get; set; }
      public DbSet<AeroDriverSetting> AeroDriverSettings { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            // ⭐ Module schema
            modelBuilder.HasDefaultSchema(Schema);

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

                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(BaseEntity.guid))
                              .HasDefaultValueSql(guidSql)
                              .ValueGeneratedOnAdd();
                  }

            }

            // Releation

            modelBuilder.Entity<PasswordRule>()
                  .HasMany(x => x.weaks)
                  .WithOne(x => x.password_rule)
                  .HasForeignKey(x => x.password_rule_guid)
                  .HasPrincipalKey(x => x.guid)
                  .OnDelete(DeleteBehavior.Cascade);

            // Seed Data

            modelBuilder.Entity<PasswordRule>()
            .HasData(
            new PasswordRule { id = 1, guid = new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6"), len = 4, is_digit = false, is_lower = false, is_symbol = false, is_upper = false }
            );

            modelBuilder.Entity<WeakPassword>()
                .HasData(
                new WeakPassword { id = 1, guid = new Guid("f371dff7-fa82-4a0f-95ba-f24954cf73f7"), pattern = "P@ssw0rd", password_rule_guid = new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6") },
                new WeakPassword { id = 2, guid = new Guid("c347ec2d-17e7-4048-82df-9b1b65730669"), pattern = "password", password_rule_guid = new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6") },
                new WeakPassword { id = 3, guid = new Guid("b3124c81-3c54-46b3-bafd-a945854fc946"), pattern = "admin", password_rule_guid = new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6") },
                new WeakPassword { id = 4, guid = new Guid("df75695c-6821-49ad-a857-60e1b0763329"), pattern = "123456", password_rule_guid = new Guid("ae243161-6067-47d0-8bcc-1990388bb6e6") }
                );

            modelBuilder.Entity<AeroDriverSetting>()
            .HasData(
                  new AeroDriverSetting
                  {
                        id = 1,
                        n_port = 1024,
                        n_scps = 1024,
                        c_type = 7,
                        c_port = 3333,
                        n_msp1_port = 3,
                        n_trasaction = 60000,
                        n_sio = 33,
                        n_mp = 615,
                        n_cp = 388,
                        n_acr = 64,
                        n_alvl = 32000,
                        n_trgr = 1024,
                        n_proc = 1024,
                        gmt_offset = -25200,
                        is_daylight_saving = false,
                        n_tz = 255,
                        n_hol = 255,
                        n_mpg = 128,
                        n_tran_limit = 60000,
                        n_cards = 200000,
                        n_alvl_per_card = 8,
                        pin_duress_mode = 2,
                        duress_const_digit = 5,
                        pin_digit = 6,
                        issue_code_bit = 1,
                        apb_location = true,
                        store_act_date = 2,
                        store_deact_date = 2,
                        used_limit = true,
                        apb_time=true,
                        host_timeout=5,
                        escort_timeout = 15,
                        multi_card_timeout = 15
                  }
            );



            // modelBuilder.Entity<CardFormat>()
            // .HasData(
            //       new CardFormat
            //       {
            //             id=1,
            //             name="26-bit Wiegand",
            //             component_id=0,
            //             fac=-1,
            //             offset=0,
            //             function_id=1,
            //             flag=0,
            //             bits=26,
            //             pe_ln=0,
            //             pe_loc=-1,
            //             po_ln=0,
            //             po_loc=-1,
            //             fc_ln=0,
            //             fc_loc=-1,
            //             ch_ln=26,
            //             ch_loc=0,
            //             ic_ln=0,
            //             ic_loc=-1,
            //             location_id = 0,
            //             is_active=true,
            //             is_default =true

            //       },
            //       new CardFormat
            //       {
            //             id=2,
            //             name="32-bit Wiegand",
            //             component_id=1,
            //             fac=-1,
            //             offset=0,
            //             function_id=1,
            //             flag=0,
            //             bits=32,
            //             pe_ln=0,
            //             pe_loc=-1,
            //             po_ln=0,
            //             po_loc=-1,
            //             fc_ln=0,
            //             fc_loc=-1,
            //             ch_ln=32,
            //             ch_loc=0,
            //             ic_ln=0,
            //             ic_loc=-1,
            //             location_id = 0,
            //             is_active=true,
            //              is_default =true

            //       },
            //       new CardFormat
            //       {
            //             id=3,
            //             name="37-bit Wiegand",
            //             component_id=2,
            //             fac=-1,
            //             offset=0,
            //             function_id=1,
            //             flag=0,
            //             bits=37,
            //             pe_ln=0,
            //             pe_loc=-1,
            //             po_ln=0,
            //             po_loc=-1,
            //             fc_ln=0,
            //             fc_loc=-1,
            //             ch_ln=37,
            //             ch_loc=0,
            //             ic_ln=0,
            //             ic_loc=-1,
            //             location_id = 0,
            //             is_active=true,
            //              is_default =true

            //       }
            // );
      }

}