using SharedKernel.Domain;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Reader : BaseEntity
{
      public int reader_number { get; set; }
      public int module_id { get; set; }
      public Module module { get; set; } = default!;
      public Reader(){}
            public Reader(Domain.Entities.Reader domain) : base(
            0,
            domain.LocationId,
            domain.IsActive,
            false
      )
      {
            this.reader_number = domain.ReaderNumber;
            this.module_id = domain.ModuleId;
      
      }
}