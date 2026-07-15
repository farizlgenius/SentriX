using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Relay 
{
      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public int relay_number { get; set; }
      public Guid module_guid { get; set; }
      public Module module { get; set; } = default!;
      public int LocationId { get; set; }

      public Relay(){}

      public Relay(Domain.Entities.Relay domain) 
      {
            this.guid = domain.Guid;
            this.relay_number = domain.RelayNumber;
            this.module_guid = domain.ModuleGuid;
            this.LocationId = domain.LocationId;

      }

}