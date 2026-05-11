using System;
using SharedKernel.Domain;

namespace Location.Infrastructure.Persistences.Entities;

public sealed class Locations : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public int country_id { get; set; }
      public Country country { get; set; } = default!;
      public Locations() { }
      
      public Locations(Location.Domain.Entities.Locations location)
      {
            name = location.Name;
            description = location.Description;
            country_id = location.CountryId;
            created_at = DateTime.UtcNow;
            updated_at = DateTime.UtcNow;
      }

      public void Update(Location.Domain.Entities.Locations location)
      {
            name = location.Name;
            description = location.Description;
            country_id = location.CountryId;
            updated_at = DateTime.UtcNow;
      }
}
