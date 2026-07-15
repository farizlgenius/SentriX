using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Reader 
{
      [Key]
      public int id {get; set;}
      public Guid guid {get; set;}
      public int reader_number { get; set; }
      public Guid module_guid { get; set; }
      public Module module { get; set; } = default!;
      public int location_id {get; set;} 
      public Reader(){}
            public Reader(Domain.Entities.Reader domain)
      {
            this.guid = domain.Guid;
            this.reader_number = domain.ReaderNumber;
            this.module_guid = domain.ModuleGuid;
            this.location_id = domain.LocationId;
      
      }
}