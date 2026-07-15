using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Input 
{
      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public int input_number { get; private set; }
      public Guid module_guid { get; private set; }
      public Module module { get; private set; } = default!;
      public int location_id { get; set; }
      public Input(){}
      public Input(Domain.Entities.Input domain)
      {
            this.guid = domain.Guid;
            this.input_number = domain.InputNumber;
            this.module_guid = domain.ModuleGuid;
            this.location_id = domain.LocationId;
      
      }
}