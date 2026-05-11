using System;
using SharedKernel.Domain;

namespace Location.Infrastructure.Persistences.Entities;

public sealed class Country : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string code { get; set; } = string.Empty;
      public ICollection<Locations> locations { get; set; } = new List<Locations>();
      public Country() { }

}
