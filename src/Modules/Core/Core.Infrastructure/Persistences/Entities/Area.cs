using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Area : BaseEntity,IAuditableEntity
{
      public AreaMultiOccMode multi_occ_mode {get; set; }
      public AreaMode area_mode {get; set; }
      public short occ_max {get; set; }
      public short occ_down {get; set; }
      public short occ_up {get; set; }
      public string metadata {get; set; } = string.Empty;

      // Relation
      public ICollection<AreaDoorIn> doors_in {get; set ;} = default!;
      public ICollection<AreaDoorOut> doors_out {get; set; }= default!;
      public int device_id {get; set; }
      public Device device {get; set; } = default!;

      public Area(){}
      public Area(Domain.Entities.Domain d) : base(d.Guid,d.ExternalId)
      {
            name = d.Name;
            multi_occ_mode = d.MultiOccMode;
            area_mode = d.AreaMode;
            occ_max = d.OccMax;
            occ_down = d.OccDown;
            occ_up = d.OccUp;
            metadata = d.Metadata;
            if(d.DeviceId != 0)
                  device_id = d.DeviceId;
      }
}