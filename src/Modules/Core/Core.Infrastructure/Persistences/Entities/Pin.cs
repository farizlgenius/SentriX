using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Pin : BaseEntity
{

      public string pin { get; set; } = string.Empty;
      public int? user_id { get; set; }
      public User user { get; set; } = default!;

      public Pin() { }

      public Pin(Domain.Entities.Pin d) : base(d.Guid)
      {
            this.pin = d.Pins;
      }

}