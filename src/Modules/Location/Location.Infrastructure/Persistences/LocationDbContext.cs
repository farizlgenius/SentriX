using System;
using Location.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Location.Infrastructure.Persistences;

public sealed class LocationDbContext(DbContextOptions<LocationDbContext> options) : DbContext(options)
{

      public const string Schema = "location";

      public DbSet<Locations> Locations { get; set; }
      public DbSet<Country> Countries { get; set; }


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

            modelBuilder.Entity<Locations>()
                  .HasOne(l => l.country)
                  .WithMany(c => c.locations)
                  .HasForeignKey(l => l.country_id)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Locations>()
                  .HasData(
                        new Locations { id = 1, name = "Main Location", description = "Main location descriptions", country_id = 178 }
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
                  new Country {id=178, name = "Default", code = "" }
                  );


      }
}
