using System.Security.Cryptography.X509Certificates;
using Core.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences;

public sealed class CoreDbContext(DbContextOptions<CoreDbContext> options) : DbContext(options)
{
      public const string Schema = "core";
      public DbSet<Location> Locations { get; set; }
      public DbSet<Country> Countries { get; set; }
      public DbSet<Device> Devices { get; set; }
      public DbSet<DeviceModule> DeviceModules { get; set; }
      public DbSet<Company> Companies { get; set; }
      public DbSet<Department> Departments { get; set; }
      public DbSet<Position> Positions { get; set; }
      public DbSet<User> Users { get; set; }
      public DbSet<UserAdditional> UserAdditionals { get; set; }
      public DbSet<Card> Cards { get; set; }
      public DbSet<Pin> Pins { get; set; }
      public DbSet<LicensePlate> LicensePlates { get; set; }
      public DbSet<QrCode> QrCodes { get; set; }
      public DbSet<Face> Faces { get; set; }
      public DbSet<Feature> Features { get; set; }
      public DbSet<FeaturePermission> FeaturePermissions { get; set; }
      public DbSet<ModulePermission> ModulePermissions { get; set; }
      public DbSet<Role> Roles { get; set; }
      public DbSet<Module> Modules { get; set; }
      public DbSet<UserLocation> UserLocations { get; set; }
      public DbSet<ComponentMapping> ComponentMappings { get; set; }
      public DbSet<Group> Groups { get; set; }
      public DbSet<UserGroup> UserGroups { get; set; }
      public DbSet<Operator> Operators { get; set; }
      public DbSet<OperatorLocation> OperatorLocations { get; set; }
      public DbSet<Holiday> Holidays { get; set; }
      public DbSet<Persistences.Entities.TimeZone> TimeZones { get; set; }
      public DbSet<TimeZoneInterval> TimeZoneIntervals { get; set; }
      public DbSet<Interval> Intervals { get; set; }
      public DbSet<DayInWeek> DayInWeeks { get; set; }
      public DbSet<Door> Doors { get; set; }
      public DbSet<Reader> Readers { get; set; }
      public DbSet<Sensor> Sensors { get; set; }
      public DbSet<BreakGlass> BreakGlasses { get; set; }
      public DbSet<Rex> Rexes { get; set; }
      public DbSet<Relay> Relays { get; set; }
      public DbSet<Buzzer> Buzzers { get; set; }
      public DbSet<Lane> Lanes { get; set; }
      public DbSet<Turnstile> Turnstiles { get; set; }
      public DbSet<GroupComponent> GroupComponents { get; set; }
      public DbSet<Output> Outputs { get; set; }
      public DbSet<Output> Inputs { get; set; }
      public DbSet<Event> Events { get; set; }
      public DbSet<AdapterEvent> AdapterEvents { get; set; }
      public DbSet<ExceptionEvent> ExceptionEvent { get; set; }
      public DbSet<AuditTrail> AuditTrails { get; set; }
      public DbSet<CardFormat> CardFormats { get; set; }
      public DbSet<Procedure> Procedures { get; set; }
      public DbSet<Persistences.Entities.Action> Action { get; set; }
      public DbSet<ProcedureAction> ProcedureActions { get; set; }
      public DbSet<TriggerSource> TriggerSources { get; set; }
      public DbSet<TriggerType> TriggerTypes { get; set; }
      public DbSet<TriggerCode> TriggerCodes { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            Console.WriteLine("=== Entities ===");

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                  Console.WriteLine(entity.ClrType.FullName);
            }

            base.OnModelCreating(modelBuilder);
            // ⭐ Module schema
            modelBuilder.HasDefaultSchema(Schema);

            // // BaseEntity supplies shared columns only. Map each concrete derived
            // // entity to its own table rather than creating a table for the
            // // inheritance root (the default TPH strategy).
            // modelBuilder.Entity<BaseEntity>().UseTpcMappingStrategy();

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

            // Trigger
            modelBuilder.Entity<TriggerSource>()
                  .Property(x => x.guid)
                  .HasDefaultValueSql(guidSql)
                  .ValueGeneratedOnAdd();

            modelBuilder.Entity<TriggerType>()
                  .Property(x => x.guid)
                  .HasDefaultValueSql(guidSql)
                  .ValueGeneratedOnAdd();

            modelBuilder.Entity<TriggerCode>()
                  .Property(x => x.guid)
                  .HasDefaultValueSql(guidSql)
                  .ValueGeneratedOnAdd();

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

            modelBuilder.Entity<User>()
            .Property(o => o.title)
            .HasConversion<string>();

            modelBuilder.Entity<User>()
            .Property(o => o.gender)
            .HasConversion<string>();

            modelBuilder.Entity<Operator>()
            .Property(o => o.title)
            .HasConversion<string>();

            modelBuilder.Entity<Operator>()
            .Property(o => o.gender)
            .HasConversion<string>();

            modelBuilder.Entity<Device>()
            .Property(o => o.vendor)
            .HasConversion<string>();

            modelBuilder.Entity<DeviceModule>()
            .Property(o => o.model)
            .HasConversion<string>();

            modelBuilder.Entity<Event>()
            .Property(o => o.vendor)
            .HasConversion<string>();

            modelBuilder.Entity<AuditTrail>()
            .Property(o => o.action)
            .HasConversion<string>();

            modelBuilder.Entity<ComponentMapping>()
            .Property(o => o.entity)
            .HasConversion<string>();

            modelBuilder.Entity<AuditTrail>()
            .Property(o => o.entity)
            .HasConversion<string>();

            modelBuilder.Entity<Persistences.Entities.Action>()
            .Property(o => o.type)
            .HasConversion<string>();


            modelBuilder.Entity<AdapterEvent>(b =>
           {
                 b.Property(d => d.vendor).HasConversion<string>();
                 b.Property(d => d.status).HasConversion<string>();
           });

            // Door Enums
            modelBuilder.Entity<Door>(b =>
            {
                  b.Property(d => d.vendor).HasConversion<string>();
                  b.Property(d => d.type).HasConversion<string>();
            });

            // Reader Enums
            modelBuilder.Entity<Reader>(b =>
            {
                  b.Property(x => x.reader_direction).HasConversion<string>();
                  b.Property(x => x.mode).HasConversion<string>();
                  b.Property(x => x.vendor).HasConversion<string>();
            });

            // Rex Enums
            modelBuilder.Entity<Rex>(b =>
            {
                  b.Property(x => x.vendor).HasConversion<string>();

            });

            // Relay Enums
            modelBuilder.Entity<Relay>(b =>
            {
                  b.Property(x => x.vendor).HasConversion<string>();

            });

            // Sensor Enums
            modelBuilder.Entity<Sensor>(b =>
            {
                  b.Property(x => x.vendor).HasConversion<string>();

            });

            modelBuilder.Entity<BreakGlass>(b =>
           {
                 b.Property(x => x.vendor).HasConversion<string>();
           });

            // Sensor Enums
            modelBuilder.Entity<Output>(b =>
            {
                  b.Property(x => x.vendor).HasConversion<string>();

            });

            modelBuilder.Entity<Input>(b =>
            {
                  b.Property(x => x.vendor).HasConversion<string>();

            });


            // Indexing and key setting 
            modelBuilder.Entity<Card>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<Operator>()
            .HasIndex(
                  x => new
                  {
                        x.guid,
                        x.id,
                        x.username
                  }
            )
            .IsUnique();

            modelBuilder.Entity<Company>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<ComponentMapping>()
            .HasIndex(x => new
            {
                  x.guid,
                  x.external_id,
                  x.mac,
                  x.vendor
            });

            modelBuilder.Entity<Country>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<Department>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<Device>()
            .HasIndex(
                  x => new
                  {
                        x.guid,
                        x.mac,
                        x.vendor
                  }
            ).IsUnique();

            modelBuilder.Entity<DeviceModule>()
            .HasIndex(
                  x => new
                  {
                        x.device_id,
                        x.mac,
                        x.guid,
                        x.location_id
                  }
            ).IsUnique();

            modelBuilder.Entity<Face>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<Feature>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<License>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<LicenseKey>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<LicensePlate>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<Location>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<UserLocation>()
            .HasIndex(
                  x => new
                  {
                        x.user_id,
                        x.location_id
                  }
            ).IsUnique();


            modelBuilder.Entity<FeaturePermission>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<Pin>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<Position>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<QrCode>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(
                  x => new
                  {
                        x.guid,
                        x.username,
                        x.identification,
                        x.firstname,
                        x.lastname,
                        x.is_default
                  }
            ).IsUnique();

            modelBuilder.Entity<Role>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<UserAdditional>()
            .HasIndex(
                  x => x.guid
            ).IsUnique();

            modelBuilder.Entity<Persistences.Entities.TimeZone>()
            .HasIndex(
                  x => new
                  {
                        x.location_id,
                        x.guid,
                        x.name
                  }
            ).IsUnique();

            modelBuilder.Entity<Door>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.device_id
                        }
                  ).IsUnique();

            modelBuilder.Entity<Reader>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.door_id,
                              x.device_module_id,
                              x.slot_no
                        }
                  ).IsUnique();

            modelBuilder.Entity<Sensor>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.door_id,
                              x.device_module_id,
                              x.slot_no
                        }
                  ).IsUnique();

            modelBuilder.Entity<BreakGlass>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.door_id,
                              x.device_module_id,
                              x.slot_no
                        }
                  ).IsUnique();

            modelBuilder.Entity<Relay>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.door_id,
                              x.device_module_id,
                              x.slot_no
                        }
                  ).IsUnique();

            modelBuilder.Entity<Buzzer>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.door_id,
                              x.device_module_id,
                              x.slot_no
                        }
                  ).IsUnique();

            modelBuilder.Entity<Rex>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.door_id,
                              x.device_module_id,
                              x.slot_no
                        }
                  ).IsUnique();

            modelBuilder.Entity<GroupComponent>()
                  .HasIndex(
                        x => new
                        {
                              x.door_id,
                              x.id,
                              x.timezone_id,
                              x.guid
                        }
                  ).IsUnique();

            modelBuilder.Entity<Group>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id
                        }
                  ).IsUnique();

            modelBuilder.Entity<Output>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.device_module_id,
                              x.slot_no
                        }
                  ).IsUnique();

            modelBuilder.Entity<Input>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.device_module_id,
                              x.slot_no
                        }
                  ).IsUnique();

            modelBuilder.Entity<Event>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.location_id,
                              x.created_at,
                              x.timestamp,
                              x.mac,
                              x.vendor
                        }
                  ).IsUnique();

            modelBuilder.Entity<AdapterEvent>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.location_id,
                              x.created_at,
                              x.mac,
                              x.send_at,
                              x.received_at,
                              x.tag,
                              x.vendor
                        }
                  ).IsUnique();

            modelBuilder.Entity<AuditTrail>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.created_at,
                              x.action,
                              x.username,
                              x.entity
                        }
                  ).IsUnique();

            modelBuilder.Entity<Procedure>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.device_id,
                              x.location_id
                        }
                  ).IsUnique();

            modelBuilder.Entity<Persistences.Entities.Action>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id,
                              x.type,
                              x.device_id,
                              x.location_id
                        }
                  ).IsUnique();

            modelBuilder.Entity<Persistences.Entities.ProcedureAction>()
                  .HasIndex(
                        x => new
                        {
                              x.procedure_id,
                              x.action_id
                        }
                  ).IsUnique();

            modelBuilder.Entity<Persistences.Entities.TriggerSource>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id
                        }
                  ).IsUnique();

            modelBuilder.Entity<Persistences.Entities.TriggerCode>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id
                        }
                  ).IsUnique();

            modelBuilder.Entity<Persistences.Entities.TriggerType>()
                  .HasIndex(
                        x => new
                        {
                              x.guid,
                              x.id
                        }
                  ).IsUnique();

            // Configure relationships 

            // Location

            modelBuilder.Entity<Location>()
                  .HasOne(l => l.country)
                  .WithMany(c => c.locations)
                  .HasForeignKey(l => l.country_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.timezones)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.holidays)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.intervals)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.devices)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.modules)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.component_mappings)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.component_mappings)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLocation>()
                  .HasOne(x => x.user)
                  .WithMany(x => x.user_locations)
                  .HasForeignKey(x => x.user_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.modules)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.doors)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.groups)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.outputs)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.inputs)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.events)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.adapter_events)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.procedures)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                  .HasMany(x => x.actions)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<UserLocation>()
                        .HasOne(x => x.location)
                        .WithMany(x => x.user_locations)
                        .HasForeignKey(x => x.location_id)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OperatorLocation>()
                        .HasOne(x => x.@operator)
                        .WithMany(x => x.operator_locations)
                        .HasForeignKey(x => x.operator_id)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OperatorLocation>()
                        .HasOne(x => x.location)
                        .WithMany(x => x.operator_locations)
                        .HasForeignKey(x => x.location_id)
                        .OnDelete(DeleteBehavior.Cascade);


            // Device 

            modelBuilder.Entity<Device>()
                  .HasMany(x => x.device_modules)
                  .WithOne(x => x.device)
                  .HasForeignKey(x => x.device_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Device>()
                  .HasMany(x => x.doors)
                  .WithOne(x => x.device)
                  .HasForeignKey(x => x.device_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Device>()
                  .HasMany(x => x.procedures)
                  .WithOne(x => x.device)
                  .HasForeignKey(x => x.device_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Device>()
                  .HasMany(x => x.actions)
                  .WithOne(x => x.device)
                  .HasForeignKey(x => x.device_id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Device Module
            modelBuilder.Entity<DeviceModule>()
                  .HasMany(x => x.readers)
                  .WithOne(x => x.device_module)
                  .HasForeignKey(x => x.device_module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceModule>()
                  .HasMany(x => x.sensors)
                  .WithOne(x => x.device_module)
                  .HasForeignKey(x => x.device_module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceModule>()
                  .HasMany(x => x.break_glasses)
                  .WithOne(x => x.device_module)
                  .HasForeignKey(x => x.device_module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceModule>()
                  .HasMany(x => x.relays)
                  .WithOne(x => x.device_module)
                  .HasForeignKey(x => x.device_module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceModule>()
                  .HasMany(x => x.rexes)
                  .WithOne(x => x.device_module)
                  .HasForeignKey(x => x.device_module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceModule>()
                  .HasMany(x => x.buzzers)
                  .WithOne(x => x.device_module)
                  .HasForeignKey(x => x.device_module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceModule>()
                  .HasMany(x => x.outputs)
                  .WithOne(x => x.device_module)
                  .HasForeignKey(x => x.device_module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceModule>()
                  .HasMany(x => x.inputs)
                  .WithOne(x => x.device_module)
                  .HasForeignKey(x => x.device_module_id)
                  .OnDelete(DeleteBehavior.Cascade);


            // User

            modelBuilder.Entity<User>()
                  .HasMany(x => x.cards)
                  .WithOne(x => x.user)
                  .HasForeignKey(x => x.user_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                  .HasOne(x => x.license_plate)
                  .WithOne(x => x.user)
                  .HasForeignKey<User>(x => x.license_plate_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                  .HasOne(x => x.qr_code)
                  .WithOne(x => x.user)
                  .HasForeignKey<User>(x => x.qr_code_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                  .HasOne(x => x.pin)
                  .WithOne(x => x.user)
                  .HasForeignKey<User>(x => x.pin_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                  .HasOne(x => x.face)
                  .WithOne(x => x.user)
                  .HasForeignKey<User>(x => x.face_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                  .HasMany(x => x.additionals)
                  .WithOne(x => x.user)
                  .HasForeignKey(x => x.user_id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Module
            modelBuilder.Entity<Module>()
                  .HasMany(x => x.features)
                  .WithOne(x => x.module)
                  .HasForeignKey(x => x.module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Module>()
                  .HasMany(x => x.module_permissions)
                  .WithOne(x => x.module)
                  .HasForeignKey(x => x.module_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feature>()
                  .HasMany(x => x.feature_permission)
                  .WithOne(x => x.feature)
                  .HasForeignKey(x => x.feature_id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Role

            modelBuilder.Entity<Role>()
                  .HasMany(x => x.users)
                  .WithOne(x => x.role)
                  .HasForeignKey(x => x.role_id)
                  .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Role>()
                  .HasMany(x => x.operators)
                  .WithOne(x => x.role)
                  .HasForeignKey(x => x.role_id)
                  .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Role>()
                  .HasMany(x => x.module_permission)
                  .WithOne(x => x.role)
                  .HasForeignKey(x => x.role_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ModulePermission>()
                  .HasMany(x => x.feature_permissions)
                  .WithOne(x => x.module_permission)
                  .HasForeignKey(x => x.module_permission_id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Company

            modelBuilder.Entity<Company>()
                  .HasMany(x => x.users)
                  .WithOne(x => x.company)
                  .HasForeignKey(x => x.company_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
                  .HasMany(x => x.users)
                  .WithOne(x => x.department)
                  .HasForeignKey(x => x.department_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Position>()
                  .HasMany(x => x.users)
                  .WithOne(x => x.position)
                  .HasForeignKey(x => x.position_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
                  .HasMany(x => x.departments)
                  .WithOne(x => x.company)
                  .HasForeignKey(x => x.company_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
                  .HasMany(x => x.positions)
                  .WithOne(x => x.department)
                  .HasForeignKey(x => x.department_id)
                  .OnDelete(DeleteBehavior.Cascade);


            // Time 
            modelBuilder.Entity<Persistences.Entities.TimeZone>()
                  .HasMany(x => x.rexes)
                  .WithOne(x => x.timezone)
                  .IsRequired(false)
                  .HasForeignKey(x => x.timezone_id)
                  .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<TimeZoneInterval>()
                  .HasOne(x => x.timezone)
                  .WithMany(x => x.timezone_intervals)
                  .HasForeignKey(x => x.timezone_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TimeZoneInterval>()
                  .HasOne(x => x.interval)
                  .WithMany(x => x.timezone_intervals)
                  .HasForeignKey(x => x.interval_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Interval>()
                  .HasOne(x => x.day)
                  .WithOne(x => x.interval)
                  .HasForeignKey<Interval>(x => x.day_id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Door and component

            modelBuilder.Entity<Door>()
                  .HasMany(x => x.readers)
                  .WithOne(x => x.door)
                  .HasForeignKey(x => x.door_id)
                  .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Door>()
                  .HasMany(x => x.rexes)
                  .WithOne(x => x.door)
                  .HasForeignKey(x => x.door_id)
                  .OnDelete(DeleteBehavior.Cascade);


            // Sensor configuration
            modelBuilder.Entity<Door>()
            .HasOne(x => x.sensor)
            .WithOne(x => x.door)
            .HasForeignKey<Sensor>(x => x.door_id) // FK is now on Sensor
            .OnDelete(DeleteBehavior.Cascade);

            // Relay configuration
            modelBuilder.Entity<Door>()
            .HasOne(x => x.relay)
            .WithOne(x => x.door)
            .HasForeignKey<Relay>(x => x.door_id) // FK is now on Relay
            .OnDelete(DeleteBehavior.Cascade);

            // Bg configuration
            modelBuilder.Entity<Door>()
            .HasOne(x => x.bg)
            .WithOne(x => x.door)
            .HasForeignKey<BreakGlass>(x => x.door_id) // FK is now on Bg
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Door>()
            .HasOne(x => x.buzzer)
            .WithOne(x => x.door)
            .HasForeignKey<Door>(x => x.buzzer_id)
            .OnDelete(DeleteBehavior.Cascade);

            // Turnstile

            modelBuilder.Entity<Turnstile>()
                  .HasMany(x => x.lanes)
                  .WithOne(x => x.turnstile)
                  .HasForeignKey(x => x.turnstile_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lane>()
                  .HasMany(x => x.readers)
                  .WithOne(x => x.lane)
                  .HasForeignKey(x => x.lane_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lane>()
                  .HasOne(x => x.sensor)
                  .WithOne(x => x.lane)
                  .HasForeignKey<Lane>(x => x.sensor_id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Group
            modelBuilder.Entity<Group>()
                  .HasMany(x => x.components)
                  .WithOne(x => x.group)
                  .HasForeignKey(x => x.group_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                  .HasMany(x => x.user_groups)
                  .WithOne(x => x.group)
                  .HasForeignKey(x => x.group_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupComponent>()
                  .HasOne(x => x.door)
                  .WithMany(x => x.group_components)
                  .HasForeignKey(x => x.door_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupComponent>()
                  .HasOne(x => x.timezone)
                  .WithMany(x => x.group_components)
                  .HasForeignKey(x => x.timezone_id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Procedure && Action
            modelBuilder.Entity<Procedure>()
                  .HasMany(x => x.procedure_actions)
                  .WithOne(x => x.procedure)
                  .HasForeignKey(x => x.procedure_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Persistences.Entities.Action>()
                  .HasMany(x => x.procedure_actions)
                  .WithOne(x => x.action)
                  .HasForeignKey(x => x.action_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TriggerSource>()
                  .HasMany(x => x.types)
                  .WithOne(x => x.trigger_source)
                  .HasForeignKey(x => x.trigger_source_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TriggerType>()
                  .HasMany(x => x.codes)
                  .WithOne(x => x.trigger_type)
                  .HasForeignKey(x => x.trigger_type_id)
                  .OnDelete(DeleteBehavior.Cascade);


            // Seed Default data

            modelBuilder.Entity<Location>()
                  .HasData(
                        new Location { id = 1, guid = new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6"), name = "Main Location", description = "Main location", country_id = 178, is_default = true, is_active = true }
                  );

            modelBuilder.Entity<Country>().HasData(
                  new Country { id = 1, name = "Andorra", code = "AD" },
                  new Country { id = 2, name = "United Arab Emirates", code = "AE" },
                  new Country { id = 3, name = "Afghanistan", code = "AF" },
                  new Country { id = 4, name = "Antigua and Barbuda", code = "AG" },
                  new Country { id = 5, name = "Anguilla", code = "AI" },
                  new Country { id = 6, name = "Albania", code = "AL" },
                  new Country { id = 7, name = "Armenia", code = "AM" },
                  new Country { id = 8, name = "Netherlands Antilles", code = "AN" },
                  new Country { id = 9, name = "Angola", code = "AO" },
                  new Country { id = 10, name = "Antarctica", code = "AQ" },
                  new Country { id = 11, name = "Argentina", code = "AR" },
                  new Country { id = 12, name = "American Samoa", code = "AS" },
                  new Country { id = 13, name = "Austria", code = "AT" },
                  new Country { id = 14, name = "Australia", code = "AU" },
                  new Country { id = 15, name = "Aruba", code = "AW" },
                  new Country { id = 16, name = "Azerbaijan", code = "AZ" },
                  new Country { id = 17, name = "Bosnia and Herzegovina", code = "BA" },
                  new Country { id = 18, name = "Barbados", code = "BB" },
                  new Country { id = 19, name = "Bangladesh", code = "BD" },
                  new Country { id = 20, name = "Belgium", code = "BE" },
                  new Country { id = 21, name = "Burkina Faso", code = "BF" },
                  new Country { id = 22, name = "Bulgaria", code = "BG" },
                  new Country { id = 23, name = "Bahrain", code = "BH" },
                  new Country { id = 24, name = "Burundi", code = "BI" },
                  new Country { id = 25, name = "Benin", code = "BJ" },
                  new Country { id = 26, name = "Bermuda", code = "BM" },
                  new Country { id = 27, name = "Brunei", code = "BN" },
                  new Country { id = 28, name = "Bolivia", code = "BO" },
                  new Country { id = 29, name = "Brazil", code = "BR" },
                  new Country { id = 30, name = "Bahamas", code = "BS" },
                  new Country { id = 31, name = "Bhutan", code = "BT" },
                  new Country { id = 32, name = "Bouvet Island", code = "BV" },
                  new Country { id = 33, name = "Botswana", code = "BW" },
                  new Country { id = 34, name = "Belarus", code = "BY" },
                  new Country { id = 35, name = "Belize", code = "BZ" },
                  new Country { id = 36, name = "Canada", code = "CA" },
                  new Country { id = 37, name = "Cocos (Keeling) Islands", code = "CC" },
                  new Country { id = 38, name = "Congo (DRC)", code = "CD" },
                  new Country { id = 39, name = "Central African Republic", code = "CF" },
                  new Country { id = 40, name = "Congo (Republic)", code = "CG" },
                  new Country { id = 41, name = "Switzerland", code = "CH" },
                  new Country { id = 42, name = "Côte d'Ivoire", code = "CI" },
                  new Country { id = 43, name = "Cook Islands", code = "CK" },
                  new Country { id = 44, name = "Chile", code = "CL" },
                  new Country { id = 45, name = "Cameroon", code = "CM" },
                  new Country { id = 46, name = "China", code = "CN" },
                  new Country { id = 47, name = "Colombia", code = "CO" },
                  new Country { id = 48, name = "Costa Rica", code = "CR" },
                  new Country { id = 49, name = "Cuba", code = "CU" },
                  new Country { id = 50, name = "Cape Verde", code = "CV" },
                  new Country { id = 51, name = "Christmas Island", code = "CX" },
                  new Country { id = 52, name = "Cyprus", code = "CY" },
                  new Country { id = 53, name = "Czech Republic", code = "CZ" },
                  new Country { id = 54, name = "Germany", code = "DE" },
                  new Country { id = 55, name = "Djibouti", code = "DJ" },
                  new Country { id = 56, name = "Denmark", code = "DK" },
                  new Country { id = 57, name = "Dominica", code = "DM" },
                  new Country { id = 58, name = "Dominican Republic", code = "DO" },
                  new Country { id = 59, name = "Algeria", code = "DZ" },
                  new Country { id = 60, name = "Ecuador", code = "EC" },
                  new Country { id = 61, name = "Estonia", code = "EE" },
                  new Country { id = 62, name = "Egypt", code = "EG" },
                  new Country { id = 63, name = "Western Sahara", code = "EH" },
                  new Country { id = 64, name = "Eritrea", code = "ER" },
                  new Country { id = 65, name = "Spain", code = "ES" },
                  new Country { id = 66, name = "Ethiopia", code = "ET" },
                  new Country { id = 67, name = "Finland", code = "FI" },
                  new Country { id = 68, name = "Fiji", code = "FJ" },
                  new Country { id = 69, name = "Falkland Islands", code = "FK" },
                  new Country { id = 70, name = "Micronesia", code = "FM" },
                  new Country { id = 71, name = "Faroe Islands", code = "FO" },
                  new Country { id = 72, name = "France", code = "FR" },
                  new Country { id = 73, name = "Gabon", code = "GA" },
                  new Country { id = 74, name = "United Kingdom", code = "GB" },
                  new Country { id = 75, name = "Grenada", code = "GD" },
                  new Country { id = 76, name = "Georgia", code = "GE" },
                  new Country { id = 77, name = "French Guiana", code = "GF" },
                  new Country { id = 78, name = "Guernsey", code = "GG" },
                  new Country { id = 79, name = "Ghana", code = "GH" },
                  new Country { id = 80, name = "Gibraltar", code = "GI" },
                  new Country { id = 81, name = "Greenland", code = "GL" },
                  new Country { id = 82, name = "Gambia", code = "GM" },
                  new Country { id = 83, name = "Guinea", code = "GN" },
                  new Country { id = 84, name = "Guadeloupe", code = "GP" },
                  new Country { id = 85, name = "Equatorial Guinea", code = "GQ" },
                  new Country { id = 86, name = "Greece", code = "GR" },
                  new Country { id = 87, name = "Guatemala", code = "GT" },
                  new Country { id = 88, name = "Guam", code = "GU" },
                  new Country { id = 89, name = "Guinea-Bissau", code = "GW" },
                  new Country { id = 90, name = "Guyana", code = "GY" },
                  new Country { id = 91, name = "Hong Kong", code = "HK" },
                  new Country { id = 92, name = "Honduras", code = "HN" },
                  new Country { id = 93, name = "Croatia", code = "HR" },
                  new Country { id = 94, name = "Haiti", code = "HT" },
                  new Country { id = 95, name = "Hungary", code = "HU" },
                  new Country { id = 96, name = "Indonesia", code = "ID" },
                  new Country { id = 97, name = "Ireland", code = "IE" },
                  new Country { id = 98, name = "Israel", code = "IL" },
                  new Country { id = 99, name = "India", code = "IN" },
                  new Country { id = 100, name = "Iraq", code = "IQ" },
                  new Country { id = 101, name = "Iran", code = "IR" },
                  new Country { id = 102, name = "Iceland", code = "IS" },
                  new Country { id = 103, name = "Italy", code = "IT" },
                  new Country { id = 104, name = "Jamaica", code = "JM" },
                  new Country { id = 105, name = "Jordan", code = "JO" },
                  new Country { id = 106, name = "Japan", code = "JP" },
                  new Country { id = 107, name = "Kenya", code = "KE" },
                  new Country { id = 108, name = "Cambodia", code = "KH" },
                  new Country { id = 109, name = "South Korea", code = "KR" },
                  new Country { id = 110, name = "Kuwait", code = "KW" },
                  new Country { id = 111, name = "Kazakhstan", code = "KZ" },
                  new Country { id = 112, name = "Laos", code = "LA" },
                  new Country { id = 113, name = "Lebanon", code = "LB" },
                  new Country { id = 114, name = "Sri Lanka", code = "LK" },
                  new Country { id = 115, name = "Liberia", code = "LR" },
                  new Country { id = 116, name = "Lesotho", code = "LS" },
                  new Country { id = 117, name = "Lithuania", code = "LT" },
                  new Country { id = 118, name = "Luxembourg", code = "LU" },
                  new Country { id = 119, name = "Latvia", code = "LV" },
                  new Country { id = 120, name = "Libya", code = "LY" },
                  new Country { id = 121, name = "Morocco", code = "MA" },
                  new Country { id = 122, name = "Monaco", code = "MC" },
                  new Country { id = 123, name = "Moldova", code = "MD" },
                  new Country { id = 124, name = "Montenegro", code = "ME" },
                  new Country { id = 125, name = "Madagascar", code = "MG" },
                  new Country { id = 126, name = "Maldives", code = "MV" },
                  new Country { id = 127, name = "Mexico", code = "MX" },
                  new Country { id = 128, name = "Malaysia", code = "MY" },
                  new Country { id = 129, name = "Mozambique", code = "MZ" },
                  new Country { id = 130, name = "Namibia", code = "NA" },
                  new Country { id = 131, name = "Nigeria", code = "NG" },
                  new Country { id = 132, name = "Netherlands", code = "NL" },
                  new Country { id = 133, name = "Norway", code = "NO" },
                  new Country { id = 134, name = "Nepal", code = "NP" },
                  new Country { id = 135, name = "New Zealand", code = "NZ" },
                  new Country { id = 136, name = "Oman", code = "OM" },
                  new Country { id = 137, name = "Panama", code = "PA" },
                  new Country { id = 138, name = "Peru", code = "PE" },
                  new Country { id = 139, name = "Philippines", code = "PH" },
                  new Country { id = 140, name = "Pakistan", code = "PK" },
                  new Country { id = 141, name = "Poland", code = "PL" },
                  new Country { id = 142, name = "Portugal", code = "PT" },
                  new Country { id = 143, name = "Qatar", code = "QA" },
                  new Country { id = 144, name = "Romania", code = "RO" },
                  new Country { id = 145, name = "Serbia", code = "RS" },
                  new Country { id = 146, name = "Russia", code = "RU" },
                  new Country { id = 147, name = "Rwanda", code = "RW" },
                  new Country { id = 148, name = "Saudi Arabia", code = "SA" },
                  new Country { id = 149, name = "Sweden", code = "SE" },
                  new Country { id = 150, name = "Singapore", code = "SG" },
                  new Country { id = 151, name = "Slovenia", code = "SI" },
                  new Country { id = 152, name = "Slovakia", code = "SK" },
                  new Country { id = 153, name = "Senegal", code = "SN" },
                  new Country { id = 154, name = "Somalia", code = "SO" },
                  new Country { id = 155, name = "Suriname", code = "SR" },
                  new Country { id = 156, name = "El Salvador", code = "SV" },
                  new Country { id = 157, name = "Syria", code = "SY" },
                  new Country { id = 158, name = "Thailand", code = "TH" },
                  new Country { id = 159, name = "Tajikistan", code = "TJ" },
                  new Country { id = 160, name = "Timor-Leste", code = "TL" },
                  new Country { id = 161, name = "Turkmenistan", code = "TM" },
                  new Country { id = 162, name = "Tunisia", code = "TN" },
                  new Country { id = 163, name = "Turkey", code = "TR" },
                  new Country { id = 164, name = "Taiwan", code = "TW" },
                  new Country { id = 165, name = "Tanzania", code = "TZ" },
                  new Country { id = 166, name = "Ukraine", code = "UA" },
                  new Country { id = 167, name = "Uganda", code = "UG" },
                  new Country { id = 168, name = "United States", code = "US" },
                  new Country { id = 169, name = "Uruguay", code = "UY" },
                  new Country { id = 170, name = "Uzbekistan", code = "UZ" },
                  new Country { id = 171, name = "Vatican City", code = "VA" },
                  new Country { id = 172, name = "Venezuela", code = "VE" },
                  new Country { id = 173, name = "Vietnam", code = "VN" },
                  new Country { id = 174, name = "Yemen", code = "YE" },
                  new Country { id = 175, name = "South Africa", code = "ZA" },
                  new Country { id = 176, name = "Zambia", code = "ZM" },
                  new Country { id = 177, name = "Zimbabwe", code = "ZW" },
                  new Country { id = 178, name = "Default", code = "" }
                  );

            // Module Data

            modelBuilder.Entity<Module>()
                  .HasData(
                        new Module { id = 1, name = "Access" },
                        new Module { id = 2, name = "Visitor" },
                        new Module { id = 3, name = "Time Attendance" }
                  );


            // Feature Data
            modelBuilder.Entity<Feature>().HasData(
                  // Access
                  new Feature { id = 1, guid = new Guid("f1f1f528-1025-44de-8512-be5f269417e8"), module_id = 1, name = "dashboard", },
                  new Feature { id = 2, guid = new Guid("62e7ede3-9152-476a-a4df-173cc16a12fe"), module_id = 1, name = "events", },
                  new Feature { id = 3, guid = new Guid("c164d952-6649-49bb-95c9-2543695b8af6"), module_id = 1, name = "location", },
                  new Feature { id = 4, guid = new Guid("14fa8dca-521d-4e1a-a582-0159df91aea9"), module_id = 1, name = "alert", },
                  new Feature { id = 5, guid = new Guid("60239ccd-4cd7-441a-94c4-4a1577c79e38"), module_id = 1, name = "operator", },
                  new Feature { id = 6, guid = new Guid("dc76438d-0e0d-4d60-88bc-0559cb81ce4a"), module_id = 1, name = "device", },
                  new Feature { id = 7, guid = new Guid("77c0545d-ec94-4037-802f-2240bcc9020e"), module_id = 1, name = "control", },
                  new Feature { id = 8, guid = new Guid("2242b3c0-06e7-4e07-be9f-7491584c57c9"), module_id = 1, name = "monitor", },
                  new Feature { id = 9, guid = new Guid("8b5c31bb-706b-4fa5-b0f0-dd246f1e9a2b"), module_id = 1, name = "monitorgroup", },
                  new Feature { id = 10, guid = new Guid("f2143a86-d2f1-47ad-a481-c74ecbdadc83"), module_id = 1, name = "acr", },
                  new Feature { id = 11, guid = new Guid("b753863e-30f4-47aa-81b3-64dda55970da"), module_id = 1, name = "user", },
                  new Feature { id = 12, guid = new Guid("5ab363b5-a921-41e5-949e-5129eb416097"), module_id = 1, name = "group", },
                  new Feature { id = 13, guid = new Guid("4401f4d8-4145-4439-adab-d89cd3e3b2fb"), module_id = 1, name = "area", },
                  new Feature { id = 14, guid = new Guid("f18ef407-cd4d-46e8-a9f5-cbc99b87a0e4"), module_id = 1, name = "time", },
                  new Feature { id = 15, guid = new Guid("5699f80a-aa8c-4325-88cf-8ba31b85f976"), module_id = 1, name = "trigger", },
                  new Feature { id = 16, guid = new Guid("76d4a40a-3fa8-4a9f-a7b5-2b63f57fd26d"), module_id = 1, name = "map", },
                  new Feature { id = 17, guid = new Guid("24ebed55-6686-45a8-95a5-00e4e6516f4f"), module_id = 1, name = "report", },
                  new Feature { id = 18, guid = new Guid("713bcbae-1755-4a94-ab82-0180a856c80a"), module_id = 1, name = "setting", },
                  new Feature { id = 19, guid = new Guid("d57aac0d-1f61-4135-857b-cc1f51288d72"), module_id = 1, name = "tools", },
                  // Visitor
                  new Feature { id = 20, module_id = 2, name = "register" },
                  new Feature { id = 21, module_id = 2, name = "appointment" },
                  // Time
                  new Feature { id = 22, module_id = 3, name = "report" }
            );

            modelBuilder.Entity<Role>().HasData(
                  new Role { id = 1, guid = new Guid("fe527691-7b13-4294-98b5-cb95181f5453"), name = "Administrator", is_default = true, is_active = true }
            );

            modelBuilder.Entity<ModulePermission>()
            .HasData(
                  new ModulePermission { id = 1, module_id = 1, is_active = true, is_enabled = true, is_default = true, role_id = 1 },
                  new ModulePermission { id = 2, module_id = 2, is_active = true, is_enabled = true, is_default = true, role_id = 1 },
                  new ModulePermission { id = 3, module_id = 3, is_active = true, is_enabled = true, is_default = true, role_id = 1 }
            );

            modelBuilder.Entity<FeaturePermission>().HasData(
                  new FeaturePermission
                  {
                        id = 1,
                        module_permission_id = 1,
                        feature_id = 1,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 2,
                        module_permission_id = 1,
                        feature_id = 2,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 3,
                        module_permission_id = 1,
                        feature_id = 3,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 4,
                        module_permission_id = 1,
                        feature_id = 4,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 5,
                        module_permission_id = 1,
                        feature_id = 5,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 6,
                        module_permission_id = 1,
                        feature_id = 6,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 7,
                        module_permission_id = 1,
                        feature_id = 7,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 8,
                        module_permission_id = 1,
                        feature_id = 8,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 9,
                        module_permission_id = 1,
                        feature_id = 9,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 10,
                        module_permission_id = 1,
                        feature_id = 10,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 11,
                        module_permission_id = 1,
                        feature_id = 11,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 12,
                        module_permission_id = 1,
                        feature_id = 12,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 13,
                        module_permission_id = 1,
                        feature_id = 13,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 14,
                        module_permission_id = 1,
                        feature_id = 14,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 15,
                        module_permission_id = 1,
                        feature_id = 15,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 16,
                        module_permission_id = 1,
                        feature_id = 16,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 17,
                        module_permission_id = 1,
                        feature_id = 17,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 18,
                        module_permission_id = 1,
                        feature_id = 18,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 19,
                        module_permission_id = 1,
                        feature_id = 19,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 20,
                        module_permission_id = 2,
                        feature_id = 20,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 21,
                        module_permission_id = 2,
                        feature_id = 21,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  },
                  new FeaturePermission
                  {
                        id = 22,
                        module_permission_id = 3,
                        feature_id = 22,
                        is_enabled = true,
                        is_created = true,
                        is_deleted = true,
                        is_updated = true
                  }
            );


            modelBuilder.Entity<Operator>()
            .HasData(
                  new Operator
                  {
                        id = 1,
                        username = "admin",
                        password = "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=",
                        title = Title.Mr,
                        firstname = "Administrator",
                        middlename = "",
                        lastname = "System",
                        gender = Gender.Male,
                        phone = "",
                        email = "support@sentrix.com",
                        joined_date = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                        role_id = 1

                  }
            );

            modelBuilder.Entity<OperatorLocation>()
                  .HasData(
                        new OperatorLocation
                        {
                              id = 1,
                              operator_id = 1,
                              location_id = 1
                        }
                  );

            // Interval 

            // modelBuilder.Entity<Interval>()
            //       .HasData(
            //             new Interval
            //             {
            //                   id=1,
            //                   start_time=new TimeOnly(0,0),
            //                   end_time=new TimeOnly(23,59),
            //                   location_id=1,
            //                   is_default=true,
            //                   day = new DayInWeek
            //                   {
            //                         id=1,
            //                         sunday=true,
            //                         monday=true,
            //                         tuesday=true,
            //                         wednesday=true,
            //                         thursday=true,
            //                         friday=true,
            //                         saturday=true,
            //                   }
            //             },
            //             new Interval
            //             {
            //                   id=1,
            //                   start_time=new TimeOnly(0,0),
            //                   end_time=new TimeOnly(0,0),
            //                   location_id=1,
            //                   is_default=true,
            //                   day = new DayInWeek
            //                   {
            //                         id=2,
            //                         sunday=false,
            //                         monday=false,
            //                         tuesday=false,
            //                         wednesday=false,
            //                         thursday=false,
            //                         friday=false,
            //                         saturday=false,
            //                   }
            //             }
            //       );

            // Time Zone
            modelBuilder.Entity<Core.Infrastructure.Persistences.Entities.TimeZone>()
                  .HasData(
                        new Core.Infrastructure.Persistences.Entities.TimeZone
                        {
                              id = 1,
                              guid = new Guid("cff24195-43f8-42ec-a03f-e84857cc958f"),
                              name = "Always",
                              location_id = 1,
                              is_default = true,
                        },
                        new Core.Infrastructure.Persistences.Entities.TimeZone
                        {
                              id = 2,
                              guid = new Guid("614a766c-1241-4fdc-9d0d-7788291cac2d"),
                              name = "Never",
                              location_id = 1,
                              is_default = true
                        }
                  );



            // Map Timezone

            modelBuilder.Entity<Core.Infrastructure.Persistences.Entities.ComponentMapping>()
                  .HasData(
                        new Core.Infrastructure.Persistences.Entities.ComponentMapping
                        {
                              id = 1,
                              guid = new Guid("cff24195-43f8-42ec-a03f-e84857cc958f"),
                              entity = EntityType.TimeZone,
                              external_id = 1,
                              mac = null,
                              vendor = null,
                              is_default = true,
                              location_id = 1
                        },
                        new Core.Infrastructure.Persistences.Entities.ComponentMapping
                        {
                              id = 2,
                              guid = new Guid("614a766c-1241-4fdc-9d0d-7788291cac2d"),
                              entity = EntityType.TimeZone,
                              external_id = 2,
                              mac = null,
                              vendor = null,
                              is_default = true,
                              location_id = 1
                        }
                  );


            // Card Format

            modelBuilder.Entity<CardFormat>()
            .HasData(
                  new CardFormat
                  {
                        id = 1,
                        name = "26-bit Wiegand",
                        guid = new Guid("356b51d4-5650-4ef9-821b-bf38eb4ec2db"),
                        fac = -1,
                        bits = 26,
                        even_parity_len = 0,
                        even_parity_loc = -1,
                        odd_parity_len = 0,
                        odd_parity_loc = -1,
                        fac_len = 0,
                        fac_loc = -1,
                        card_no_len = 26,
                        card_no_loc = 0,
                        issue_code_len = 0,
                        issue_code_loc = -1,

                  },
                  new CardFormat
                  {
                        id = 2,
                        name = "32-bit Wiegand",
                        guid = new Guid("f04c1839-991d-4f13-be52-4a057d30af68"),
                        fac = -1,
                        bits = 32,
                        even_parity_len = 0,
                        even_parity_loc = -1,
                        odd_parity_len = 0,
                        odd_parity_loc = -1,
                        fac_len = 0,
                        fac_loc = -1,
                        card_no_len = 32,
                        card_no_loc = 0,
                        issue_code_len = 0,
                        issue_code_loc = -1

                  },
                  new CardFormat
                  {
                        id = 3,
                        name = "37-bit Wiegand",
                        guid = new Guid("2ec6aa6c-dd76-4b3a-98b3-e8b5ecff0d1e"),
                        fac = -1,
                        bits = 37,
                        even_parity_len = 0,
                        even_parity_loc = -1,
                        odd_parity_len = 0,
                        odd_parity_loc = -1,
                        fac_len = 0,
                        fac_loc = -1,
                        card_no_len = 37,
                        card_no_loc = 0,
                        issue_code_len = 0,
                        issue_code_loc = -1,

                  }
            );

            modelBuilder.Entity<ComponentMapping>()
            .HasData(
                  new ComponentMapping
                  {
                        id = 3,
                        guid = new Guid("356b51d4-5650-4ef9-821b-bf38eb4ec2db"),
                        entity = EntityType.CardFormat,
                        external_id = 0,
                        mac = null,
                        vendor = null
                  },
                  new ComponentMapping
                  {
                        id = 4,
                        guid = new Guid("f04c1839-991d-4f13-be52-4a057d30af68"),
                        entity = EntityType.CardFormat,
                        external_id = 1,
                        mac = null,
                        vendor = null
                  },
                  new ComponentMapping
                  {
                        id = 5,
                        guid = new Guid("2ec6aa6c-dd76-4b3a-98b3-e8b5ecff0d1e"),
                        entity = EntityType.CardFormat,
                        external_id = 2,
                        mac = null,
                        vendor = null
                  }
            );


            // Access Group
            modelBuilder.Entity<Group>()
                  .HasData(
                        new Group
                        {
                              id = 1,
                              name = "All Door",
                              guid = new Guid("895b0301-3d77-4ec7-8917-43260a839643"),
                              is_default = true
                        }
                  );

            modelBuilder.Entity<ComponentMapping>()
                  .HasData(
                        new ComponentMapping
                        {
                              id = 6,
                              guid = new Guid("895b0301-3d77-4ec7-8917-43260a839643"),
                              entity = EntityType.Group,
                              external_id = 1,
                              mac = null,
                              vendor = null
                        }
                  );

            // Trigger

            modelBuilder.Entity<TriggerSource>()
    .HasData(
        new TriggerSource { id = 1, name = "Device diagnostics", value = 0x00 },
        new TriggerSource { id = 2, name = "Device to HOST communication driver - not defined", value = 0x01 },
        new TriggerSource { id = 3, name = "Device local monitor points (tamper & power fault)", value = 0x02 },
        new TriggerSource { id = 4, name = "Module diagnostics", value = 0x03 },
        new TriggerSource { id = 5, name = "Module communication driver", value = 0x04 },
        new TriggerSource { id = 6, name = "Module cabinet tamper", value = 0x05 },
        new TriggerSource { id = 7, name = "Module power monitor", value = 0x06 },
        new TriggerSource { id = 8, name = "Alarm monitor point", value = 0x07 },
        new TriggerSource { id = 9, name = "Output control point", value = 0x08 },
        new TriggerSource { id = 10, name = "Access Control Reader (ACR)", value = 0x09 },
        new TriggerSource { id = 11, name = "Door: reader tamper monitor", value = 0x0A },
        new TriggerSource { id = 12, name = "Door: door position sensor", value = 0x0B },
        new TriggerSource { id = 13, name = "Door: 1st \"Request to exit\" input", value = 0x0D },
        new TriggerSource { id = 14, name = "Door: 2nd \"Request to exit\" input", value = 0x0E },
        new TriggerSource { id = 15, name = "Time zone", value = 0x0F },
        new TriggerSource { id = 16, name = "Procedure (action list)", value = 0x10 },
        new TriggerSource { id = 17, name = "Trigger", value = 0x11 },
        new TriggerSource { id = 18, name = "Trigger variable", value = 0x12 },
        new TriggerSource { id = 19, name = "Monitor point group", value = 0x13 },
        new TriggerSource { id = 20, name = "Access area", value = 0x14 },
        new TriggerSource { id = 21, name = "Door: the alternate reader's tamper monitor source_number", value = 0x15 },
        new TriggerSource { id = 22, name = "Login Service", value = 0x18 }
    );

            modelBuilder.Entity<TriggerType>()
            .HasData(
                new TriggerType { id = 1, name = "System", value = 0x01, trigger_source_id = 1 },
                new TriggerType { id = 2, name = "SIO communication status report", value = 0x02, trigger_source_id = 5 },
                new TriggerType { id = 3, name = "Binary card data", value = 0x03, trigger_source_id = 10 },
                new TriggerType { id = 4, name = "Card data", value = 0x04, trigger_source_id = 10 },
                new TriggerType { id = 5, name = "Formatted card: facility code, card number ID, issue code", value = 0x05, trigger_source_id = 10 },
                new TriggerType { id = 6, name = "Formatted card: card number only", value = 0x06, trigger_source_id = 10 },
                new TriggerType { id = 7, name = "Change-of-state", value = 0x07, trigger_source_id = 3 },
                new TriggerType { id = 8, name = "Exit request", value = 0x08, trigger_source_id = 10 },
                new TriggerType { id = 9, name = "Door status monitor change-of-state", value = 0x09, trigger_source_id = 10 },
                new TriggerType { id = 10, name = "Procedure (command list) log", value = 0x0A, trigger_source_id = 16 },
                new TriggerType { id = 11, name = "User command request report", value = 0x0B, trigger_source_id = 10 },
                new TriggerType { id = 12, name = "Change of state: trigger variable, time zone, & triggers", value = 0x0C, trigger_source_id = 15 },
                new TriggerType { id = 13, name = "ACR mode change", value = 0x0D, trigger_source_id = 10 },
                new TriggerType { id = 14, name = "Monitor point group status change", value = 0x0E, trigger_source_id = 19 },
                new TriggerType { id = 15, name = "Access area", value = 0x0F, trigger_source_id = 20 },
                new TriggerType { id = 16, name = "Extended user command", value = 0x12, trigger_source_id = 10 },
                new TriggerType { id = 17, name = "Use limit report", value = 0x13, trigger_source_id = 10 },
                new TriggerType { id = 18, name = "Web activity", value = 0x14, trigger_source_id = 1 },
                new TriggerType { id = 19, name = "Specify tranTypeCardFull (0x05) instead", value = 0x15, trigger_source_id = 10 },
                new TriggerType { id = 20, name = "Specify tranTypeCardID (0x06) instead", value = 0x16, trigger_source_id = 10 },
                new TriggerType { id = 21, name = "Operating mode change", value = 0x18, trigger_source_id = 1 },
                new TriggerType { id = 22, name = "Elevator Floor Status CoS", value = 0x1A, trigger_source_id = 10 },
                new TriggerType { id = 23, name = "File Download Status", value = 0x1B, trigger_source_id = 1 },
                new TriggerType { id = 24, name = "Elevator Floor Access Transaction", value = 0x1D, trigger_source_id = 10 },
                new TriggerType { id = 25, name = "Specify tranTypeCardFull (0x05) instead", value = 0x25, trigger_source_id = 10 },
                new TriggerType { id = 26, name = "Specify tranTypeCardID (0x06) instead", value = 0x26, trigger_source_id = 10 },
                new TriggerType { id = 27, name = "Specify tranTypeCardFull (0x05) instead", value = 0x35, trigger_source_id = 10 },
                new TriggerType { id = 28, name = "ACR extended feature stateless transition", value = 0x40, trigger_source_id = 10 },
                new TriggerType { id = 29, name = "ACR extended feature change-of-state", value = 0x41, trigger_source_id = 10 },
                //
                new TriggerType { id = 30, name = "Change-of-state", value = 0x07, trigger_source_id = 6 },
                new TriggerType { id = 31, name = "Change-of-state", value = 0x07, trigger_source_id = 7 },
                new TriggerType { id = 32, name = "Change-of-state", value = 0x07, trigger_source_id = 8 },
                new TriggerType { id = 33, name = "Change-of-state", value = 0x07, trigger_source_id = 9 },
                new TriggerType { id = 34, name = "Change-of-state", value = 0x07, trigger_source_id = 11 },
                new TriggerType { id = 35, name = "Change-of-state", value = 0x07, trigger_source_id = 13 },
                new TriggerType { id = 36, name = "Change-of-state", value = 0x07, trigger_source_id = 14 },
                new TriggerType { id = 37, name = "Change-of-state", value = 0x07, trigger_source_id = 21 },
                new TriggerType { id = 38, name = "Change-of-state", value = 0x07, trigger_source_id = 22 },
                //
                new TriggerType { id = 39, name = "Change of state: trigger variable, time zone, & triggers", value = 0x0C, trigger_source_id = 17 },
                new TriggerType { id = 40, name = "Change of state: trigger variable, time zone, & triggers", value = 0x0C, trigger_source_id = 18 }
            );

            modelBuilder.Entity<TriggerCode>()
                  .HasData(
                        // Typesys
                        new TriggerCode { id = 1, name = "SCP Power-up diagnostic" , value =1,trigger_type_id=1 },
                        new TriggerCode { id = 2, name = "Host communication offline" , value =2,trigger_type_id=1 },
                        new TriggerCode { id = 3, name = "Host communication online" , value =3,trigger_type_id=1 },
                        new TriggerCode { id = 4, name = "Transaction count exceed limit" , value =4,trigger_type_id=1 },
                        new TriggerCode { id = 5, name = "Configuration DB save complete" , value =5,trigger_type_id=1 },
                        new TriggerCode { id = 6, name = "Card database save complete" , value =6,trigger_type_id=1 },
                        new TriggerCode { id = 7, name = "Card database cleared due to SRAM buffer overflow" , value =7,trigger_type_id=1 },
                        // Type Module Comm
                        new TriggerCode { id = 8, name = "Disabled" , value =1,trigger_type_id=2 },
                        new TriggerCode { id = 9, name = "Offline:timeout" , value =2,trigger_type_id=2 },
                        new TriggerCode { id = 10, name = "Offline:invalid identificatiion from module" , value =3,trigger_type_id=2 },
                        new TriggerCode { id = 11, name = "Offline:command too long" , value =4,trigger_type_id=2 },
                        new TriggerCode { id = 12, name = "Online:normal connection" , value =5,trigger_type_id=2 },
                        new TriggerCode { id = 13, name = "hexLoad report: ser_num is address loaded (-1 = last record)" , value =6,trigger_type_id=2 },
                        // CardBin
                        new TriggerCode { id = 14, name = "Access denied, Invalid card format" , value=1,trigger_type_id=3 },
                        // CardBcd
                        new TriggerCode { id = 15, name = "Access denied, Invalid card format, forward read" , value=1,trigger_type_id=4 },
                        new TriggerCode { id = 16, name = "Access denied, Invalid card format, reverse read" , value=2,trigger_type_id=4 },
                        // Card full
                        new TriggerCode { id = 17, name = "Request rejected: access point \"locked\"" , value =1,trigger_type_id=5 },
                        new TriggerCode { id = 18, name = "Request accepted: access point \"unlocked\"" , value =2,trigger_type_id=5 },
                        new TriggerCode { id = 19, name = "Request rejected: invalid facility code" , value =3,trigger_type_id=5 },
                        new TriggerCode { id = 20, name = "Request rejected: invalid facility code extension" , value =4,trigger_type_id=5 },
                        new TriggerCode { id = 21, name = "Request rejected: not in card file" , value =5,trigger_type_id=5 },
                        new TriggerCode { id = 22, name = "Request rejected: invalid issue code" , value =6,trigger_type_id=5 },
                        new TriggerCode { id = 23, name = "Request granted: facility code verified, not used" , value =7,trigger_type_id=5 },
                        new TriggerCode { id = 24, name = "Request granted: facility code verified, door used" , value =8,trigger_type_id=5 },
                        new TriggerCode { id = 25, name = "Access denied - asked for host approval, then timed out" , value =9,trigger_type_id=5 },
                        new TriggerCode { id = 26, name = "Reporting that this card is \"about to get access granted\"" , value =10,trigger_type_id=5 },
                        new TriggerCode { id = 27, name = "Access denied count exceeded" , value =11,trigger_type_id=5 },
                        new TriggerCode { id = 28, name = "Access denied - asked for host approval, then host denied" , value =12,trigger_type_id=5 },
                        new TriggerCode { id = 29, name = "Request rejected: Airlock is busy" , value =13,trigger_type_id=5 },
                        // CardID
                        new TriggerCode { id = 30, name = "Request rejected: deactivated card", value = 1, trigger_type_id = 6 },
                        new TriggerCode { id = 31, name = "Request rejected: before activation date", value = 2, trigger_type_id = 6 },
                        new TriggerCode { id = 32, name = "Request rejected: after expiration date", value = 3, trigger_type_id = 6 },
                        new TriggerCode { id = 33, name = "Request rejected: invalid time", value = 4, trigger_type_id = 6 },
                        new TriggerCode { id = 34, name = "Request rejected: invalid PIN", value = 5, trigger_type_id = 6 },
                        new TriggerCode { id = 35, name = "Request rejected: anti-passback violation", value = 6, trigger_type_id = 6 },
                        new TriggerCode { id = 36, name = "Request granted: APB violation, not used", value = 7, trigger_type_id = 6 },
                        new TriggerCode { id = 37, name = "Request granted: APB violation, used", value = 8, trigger_type_id = 6 },
                        new TriggerCode { id = 38, name = "Request rejected: duress code detected", value = 9, trigger_type_id = 6 },
                        new TriggerCode { id = 39, name = "Request granted: duress, used", value = 10, trigger_type_id = 6 },
                        new TriggerCode { id = 40, name = "Request granted: duress, not used", value = 11, trigger_type_id = 6 },
                        new TriggerCode { id = 41, name = "Request granted: full test, not used", value = 12, trigger_type_id = 6 },
                        new TriggerCode { id = 42, name = "Request granted: full test, used", value = 13, trigger_type_id = 6 },
                        new TriggerCode { id = 43, name = "Request denied: never allowed at this reader (all Tz's = 0)", value = 14, trigger_type_id = 6 },
                        new TriggerCode { id = 44, name = "Request denied: no second card presented", value = 15, trigger_type_id = 6 },
                        new TriggerCode { id = 45, name = "Request denied: occupancy limit reached", value = 16, trigger_type_id = 6 },
                        new TriggerCode { id = 46, name = "Request denied: the area is NOT enabled", value = 17, trigger_type_id = 6 },
                        new TriggerCode { id = 47, name = "Request denied: use limit", value = 18, trigger_type_id = 6 },
                        new TriggerCode { id = 48, name = "Granting access: used/not used transaction will follow", value = 21, trigger_type_id = 6 },
                        new TriggerCode { id = 49, name = "Request rejected: no escort card presented", value = 24, trigger_type_id = 6 },
                        new TriggerCode { id = 50, name = "Reserved", value = 25, trigger_type_id = 6 },
                        new TriggerCode { id = 51, name = "Reserved", value = 26, trigger_type_id = 6 },
                        new TriggerCode { id = 52, name = "Reserved", value = 27, trigger_type_id = 6 },
                        new TriggerCode { id = 53, name = "Request rejected: airlock is busy", value = 29, trigger_type_id = 6 },
                        new TriggerCode { id = 54, name = "Request rejected: incomplete CARD & PIN sequence", value = 30, trigger_type_id = 6 },
                        new TriggerCode { id = 55, name = "Request granted: double-card event", value = 31, trigger_type_id = 6 },
                        new TriggerCode { id = 56, name = "Request granted: double-card event while in uncontrolled state (locked/unlocked)", value = 32, trigger_type_id = 6 },
                        new TriggerCode { id = 57, name = "Granting access: requires escort, pending escort card", value = 39, trigger_type_id = 6 },
                        new TriggerCode { id = 58, name = "Request rejected: violates minimum occupancy count", value = 40, trigger_type_id = 6 },
                        new TriggerCode { id = 59, name = "Request rejected: card pending at another reader", value = 41, trigger_type_id = 6 },
                        // COS
                        new TriggerCode { id = 60, name = "Disconnected", value = 1, trigger_type_id = 7 },
                        new TriggerCode { id = 61, name = "Unknown", value = 2, trigger_type_id = 7 },
                        new TriggerCode { id = 62, name = "Secure", value = 3, trigger_type_id = 7 },
                        new TriggerCode { id = 63, name = "Alarm", value = 4, trigger_type_id = 7 },
                        new TriggerCode { id = 64, name = "Fault", value = 5, trigger_type_id = 7 },
                        new TriggerCode { id = 65, name = "Exit delay in progress", value = 6, trigger_type_id = 7 },
                        new TriggerCode { id = 66, name = "Entry delay in progress", value = 7, trigger_type_id = 7 },
                        new TriggerCode { id = 67, name = "Online, reader tamper inactive", value = 0, trigger_type_id = 7 },
                        new TriggerCode { id = 68, name = "Online, reader tamper active", value = 1, trigger_type_id = 7 },
                        new TriggerCode { id = 69, name = "N/A", value = 2, trigger_type_id = 7 },
                        new TriggerCode { id = 70, name = "Communication broken (reader offline)", value = 3, trigger_type_id = 7 },
                        // REX
                        new TriggerCode { id = 176, name = "Disconnected", value = 1, trigger_type_id = 8 },
                        new TriggerCode { id = 177, name = "Exit cycle: door use not verified", value = 1, trigger_type_id = 8 },
                        new TriggerCode { id = 178, name = "Exit cycle: door not used", value = 2, trigger_type_id = 8 },
                        new TriggerCode { id = 179, name = "Exit cycle: door used", value = 3, trigger_type_id = 8 },
                        new TriggerCode { id = 71, name = "Host initiated request: door use not verified", value = 4, trigger_type_id = 8 },
                        new TriggerCode { id = 72, name = "Host initiated request: door not used", value = 5, trigger_type_id = 8 },
                        new TriggerCode { id = 73, name = "Host initiated request: door used", value = 6, trigger_type_id = 8 },
                        new TriggerCode { id = 74, name = "Exit cycle: started", value = 9, trigger_type_id = 8 },
                        // CosDoor
                        new TriggerCode { id = 75, name = "Disconnected", value = 1, trigger_type_id = 9 },
                        new TriggerCode { id = 76, name = "Unknown", value = 2, trigger_type_id = 9 },
                        new TriggerCode { id = 77, name = "Secure", value = 3, trigger_type_id = 9 },
                        new TriggerCode { id = 78, name = "Alarm (forced, held open or both)", value = 4, trigger_type_id = 9 },
                        new TriggerCode { id = 79, name = "Fault", value = 5, trigger_type_id = 9 },
                        // Procedure
                        new TriggerCode { id = 80, name = "Cancel procedure (abort delay)", value = 1, trigger_type_id = 10 },
                        new TriggerCode { id = 81, name = "Execute procedure (start new)", value = 2, trigger_type_id = 10 },
                        new TriggerCode { id = 82, name = "Resume procedure, if paused", value = 3, trigger_type_id = 10 },
                        new TriggerCode { id = 83, name = "Execute procedure with prefix 256 actions", value = 4, trigger_type_id = 10 },
                        new TriggerCode { id = 84, name = "Execute procedure with prefix 512 actions", value = 5, trigger_type_id = 10 },
                        new TriggerCode { id = 85, name = "Execute procedure with prefix 1024 actions", value = 6, trigger_type_id = 10 },
                        new TriggerCode { id = 86, name = "Resume procedure with prefix 256 actions", value = 7, trigger_type_id = 10 },
                        new TriggerCode { id = 87, name = "Resume procedure with prefix 512 actions", value = 8, trigger_type_id = 10 },
                        new TriggerCode { id = 88, name = "Resume procedure with prefix 1024 actions", value = 9, trigger_type_id = 10 },
                        new TriggerCode { id = 89, name = "Command was issued to procedure with no actions - (NOP)", value = 10, trigger_type_id = 10 },
                        // User Cmnd
                        new TriggerCode { id = 90, name = "Command entered by the user", value = 1, trigger_type_id = 11 },
                        // Type Activate 
                        new TriggerCode { id = 91, name = "Became inactive", value = 1, trigger_type_id = 12 },
                        new TriggerCode { id = 92, name = "Became active", value = 2, trigger_type_id = 12 },
                        // Door Mode
                         new TriggerCode { id = 93, name = "Disabled", value = 1, trigger_type_id = 13 },
                         new TriggerCode { id = 94, name = "Unlocked", value = 2, trigger_type_id = 13 },
                         new TriggerCode { id = 95, name = "Locked (exit request enabled)", value = 3, trigger_type_id = 13 },
                         new TriggerCode { id = 96, name = "Facility code only", value = 4, trigger_type_id = 13 },
                         new TriggerCode { id = 97, name = "Card only", value = 5, trigger_type_id = 13 },
                         new TriggerCode { id = 98, name = "PIN only", value = 6, trigger_type_id = 13 },
                         new TriggerCode { id = 99, name = "Card and PIN", value = 7, trigger_type_id = 13 },
                         new TriggerCode { id = 100, name = "PIN or card", value = 8, trigger_type_id = 13 },
                        // Type MPG
                        new TriggerCode { id = 101, name = "First disarm command executed (mask_count was 0, all MPs got masked)", value = 1, trigger_type_id = 14 },
                        new TriggerCode { id = 102, name = "Subsequent disarm command executed (mask_count incremented, MPs already masked)", value = 2, trigger_type_id = 14 },
                        new TriggerCode { id = 103, name = "Override command: armed (mask_count cleared, all points unmasked)", value = 3, trigger_type_id = 14 },
                        new TriggerCode { id = 104, name = "Override command: disarmed (mask_count set, unmasked all points)", value = 4, trigger_type_id = 14 },
                        new TriggerCode { id = 105, name = "Force arm command, MPG armed, (may have active zones, mask_count is now zero)", value = 5, trigger_type_id = 14 },
                        new TriggerCode { id = 106, name = "Force arm command, MPG not armed (mask_count decremented)", value = 6, trigger_type_id = 14 },
                        new TriggerCode { id = 107, name = "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)", value = 7, trigger_type_id = 14 },
                        new TriggerCode { id = 108, name = "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)", value = 8, trigger_type_id = 14 },
                        new TriggerCode { id = 109, name = "Standard arm command, MPG still armed, (mask_count decremented)", value = 9, trigger_type_id = 14 },
                        new TriggerCode { id = 110, name = "Override arm command, MPG armed (mask_count is now zero)", value = 10, trigger_type_id = 14 },
                        new TriggerCode { id = 111, name = "Override arm command, MPG did not arm, (mask_count decremented)", value = 11, trigger_type_id = 14 },
                        // Type Area
                        new TriggerCode { id = 112, name = "Area disabled", value = 1, trigger_type_id = 15 },
                        new TriggerCode { id = 113, name = "Area enabled", value = 2, trigger_type_id = 15 },
                        new TriggerCode { id = 114, name = "Occupancy count reached zero", value = 3, trigger_type_id = 15 },
                        new TriggerCode { id = 115, name = "Occupancy count reached the \"downward-limit\"", value = 4, trigger_type_id = 15 },
                        new TriggerCode { id = 116, name = "Occupancy count reached the \"upward-limit\"", value = 5, trigger_type_id = 15 },
                        new TriggerCode { id = 117, name = "Occupancy count reached the \"max-occupancy-limit\"", value = 6, trigger_type_id = 15 },
                        new TriggerCode { id = 118, name = "Multi-occupancy mode changed", value = 7, trigger_type_id = 15 },
                        // Type Use Limit
                        new TriggerCode { id = 119, name = "Use limit changed, reporting new limit", value = 1, trigger_type_id = 17 },
                        // Web Activity
                        new TriggerCode { id = 120, name = "Save home notes", value = 1, trigger_type_id = 18 },
                        new TriggerCode { id = 121, name = "Save network settings", value = 2, trigger_type_id = 18 },
                        new TriggerCode { id = 122, name = "Save host communication settings", value = 3, trigger_type_id = 18 },
                        new TriggerCode { id = 123, name = "Add user", value = 4, trigger_type_id = 18 },
                        new TriggerCode { id = 124, name = "Delete user", value = 5, trigger_type_id = 18 },
                        new TriggerCode { id = 125, name = "Modify user", value = 6, trigger_type_id = 18 },
                        new TriggerCode { id = 126, name = "Save password strength and session timer", value = 7, trigger_type_id = 18 },
                        new TriggerCode { id = 127, name = "Save web server options", value = 8, trigger_type_id = 18 },
                        new TriggerCode { id = 128, name = "Save time server settings", value = 9, trigger_type_id = 18 },
                        new TriggerCode { id = 129, name = "Auto save timer settings", value = 10, trigger_type_id = 18 },
                        new TriggerCode { id = 130, name = "Load certificate", value = 11, trigger_type_id = 18 },
                        new TriggerCode { id = 131, name = "Logged out by link", value = 12, trigger_type_id = 18 },
                        new TriggerCode { id = 132, name = "Logged out by timeout", value = 13, trigger_type_id = 18 },
                        new TriggerCode { id = 133, name = "Logged out by user", value = 14, trigger_type_id = 18 },
                        new TriggerCode { id = 134, name = "Logged out by apply", value = 15, trigger_type_id = 18 },
                        new TriggerCode { id = 135, name = "Invalid login", value = 16, trigger_type_id = 18 },
                        new TriggerCode { id = 136, name = "Successful login", value = 17, trigger_type_id = 18 },
                        new TriggerCode { id = 137, name = "Network diagnostic saved", value = 18, trigger_type_id = 18 },
                        new TriggerCode { id = 138, name = "Card DB size saved", value = 19, trigger_type_id = 18 },
                        new TriggerCode { id = 139, name = "Diagnostic page saved", value = 21, trigger_type_id = 18 },
                        new TriggerCode { id = 140, name = "Security options page saved", value = 22, trigger_type_id = 18 },
                        new TriggerCode { id = 141, name = "Add-on package page saved", value = 23, trigger_type_id = 18 },
                        new TriggerCode { id = 142, name = "Not used", value = 24, trigger_type_id = 18 },
                        new TriggerCode { id = 143, name = "Not used", value = 25, trigger_type_id = 18 },
                        new TriggerCode { id = 144, name = "Not used", value = 26, trigger_type_id = 18 },
                        new TriggerCode { id = 145, name = "Invalid login limit reached", value = 27, trigger_type_id = 18 },
                        new TriggerCode { id = 146, name = "Firmware download initiated", value = 28, trigger_type_id = 18 },
                        new TriggerCode { id = 147, name = "Advanced networking routes saved", value = 29, trigger_type_id = 18 },
                        new TriggerCode { id = 148, name = "Advanced networking reversion timer started", value = 30, trigger_type_id = 18 },
                        new TriggerCode { id = 149, name = "Advanced networking reversion timer elapsed", value = 31, trigger_type_id = 18 },
                        new TriggerCode { id = 150, name = "Advanced networking route changes reverted", value = 32, trigger_type_id = 18 },
                        new TriggerCode { id = 151, name = "Advanced networking route changes cleared", value = 33, trigger_type_id = 18 },
                        new TriggerCode { id = 152, name = "Certificate generation started", value = 34, trigger_type_id = 18 },
                        // Operation Mode
                        new TriggerCode { id = 153, name = "Operating mode changed to mode 0", value = 1, trigger_type_id = 21 },
                        new TriggerCode { id = 154, name = "Operating mode changed to mode 1", value = 2, trigger_type_id = 21 },
                        new TriggerCode { id = 155, name = "Operating mode changed to mode 2", value = 3, trigger_type_id = 21 },
                        new TriggerCode { id = 156, name = "Operating mode changed to mode 3", value = 4, trigger_type_id = 21 },
                        new TriggerCode { id = 157, name = "Operating mode changed to mode 4", value = 5, trigger_type_id = 21 },
                        new TriggerCode { id = 158, name = "Operating mode changed to mode 5", value = 6, trigger_type_id = 21 },
                        new TriggerCode { id = 159, name = "Operating mode changed to mode 6", value = 7, trigger_type_id = 21 },
                        new TriggerCode { id = 160, name = "Operating mode changed to mode 7", value = 8, trigger_type_id = 21 },
                        // Type CosElevator
                        new TriggerCode { id = 161, name = "Floor status is secure", value = 1, trigger_type_id = 22 },
                        new TriggerCode { id = 162, name = "Floor status is public", value = 2, trigger_type_id = 22 },
                        new TriggerCode { id = 163, name = "Floor status is disabled (override)", value = 3, trigger_type_id = 22 },
                        // Type File Download
                        new TriggerCode { id = 164, name = "File transfer success", value = 1, trigger_type_id = 23 },
                        new TriggerCode { id = 165, name = "File transfer error", value = 2, trigger_type_id = 23 },
                        new TriggerCode { id = 166, name = "File delete successful", value = 3, trigger_type_id = 23 },
                        new TriggerCode { id = 167, name = "File delete unsuccessful", value = 4, trigger_type_id = 23 },
                        new TriggerCode { id = 168, name = "OSDP file transfer complete (primary ACR) - look at source number for ACR number", value = 5, trigger_type_id = 23 },
                        new TriggerCode { id = 169, name = "OSDP file transfer error (primary ACR) - look at source number for ACR number", value = 6, trigger_type_id = 23 },
                        new TriggerCode { id = 170, name = "OSDP file transfer complete (alternate ACR) - look at source number for ACR number", value = 7, trigger_type_id = 23 },
                        new TriggerCode { id = 171, name = "OSDP file transfer error (alternate ACR) - look at source number for ACR number", value = 8, trigger_type_id = 23 },
                        // Type Cos Elevator Access
                        new TriggerCode { id = 172, name = "Elevator access", value = 1, trigger_type_id = 24 },
                        // ACR EXtend feature cos
                        new TriggerCode { id = 173, name = "Extended status updated", value = 1, trigger_type_id = 28 },
                        new TriggerCode { id = 174, name = "Secure / Inactive", value = 3, trigger_type_id = 29 },
                        new TriggerCode { id = 175, name = "Alarm / Active", value = 4, trigger_type_id = 29 }
                  );




      }
}
