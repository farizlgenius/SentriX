using System.Security.Cryptography.X509Certificates;
using Core.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences;

public sealed class CoreDbContext(DbContextOptions<CoreDbContext> options) : DbContext(options)
{
      public const string Schema = "core";
      public DbSet<Location> Locations { get; set; }
      public DbSet<Country> Countries { get; set; }
      public DbSet<Device> Devices { get; set; }
      public DbSet<SubDevice> SubDevices { get; set; }
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
                  x.internal_id,
                  x.external_id
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
                  x => x.guid
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

            modelBuilder.Entity<SubDevice>()
            .HasIndex(
                  x => new
                  {
                        x.guid,
                        x.location_id
                  }
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

            // Configure relationships 

            // Location

            modelBuilder.Entity<Location>()
                  .HasOne(l => l.country)
                  .WithMany(c => c.locations)
                  .HasForeignKey(l => l.country_id)
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
                  .HasMany(x => x.component_mapping)
                  .WithOne(x => x.location)
                  .HasForeignKey(x => x.location_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLocation>()
                  .HasOne(x => x.user)
                  .WithMany(x => x.user_locations)
                  .HasForeignKey(x => x.user_id)
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
                  .HasMany(x => x.sub_device)
                  .WithOne(x => x.device)
                  .HasForeignKey(x => x.device_id)
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

            //       modelBuilder.Entity<User>().HasData(
            //            new User
            //            {
            //                  id = 1,
            //                  guid = new Guid("ed2b5887-9dcb-43bd-a6f8-988330df5181"),
            //                  identification = "admin",
            //                  username = "admin",
            //                  password = "100000.lG1/4V/VRPZsbhf/Zqc4xw==.6vYcf+wEMSgqcaNhoZEdM9PaPxx2ZUErZhQbeMxo5OY=",
            //                  user_code = "admin01",
            //                  title = Title.Mr,
            //                  firstname = "admin",
            //                  lastname = "system",
            //                  gender = Gender.Male,
            //                  date_of_birth = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc),
            //                  email = "support@sentrix.com",
            //                  is_operator = true,
            //                  is_user = false,
            //                  role_id = 1,
            //                  active_time = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc),
            //                  expire_time = new DateTime(9999, 01, 01, 0, 0, 0, DateTimeKind.Utc),
            //                  is_default = true,
            //                  is_active = true,

            //            }
            //      );

            //       modelBuilder.Entity<UserLocation>()
            //             .HasData(
            //                   new UserLocation
            //                   {
            //                         id = 1,
            //                         user_id = 1,
            //                         location_id = 1
            //                   }
            //             );

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



      }
}
