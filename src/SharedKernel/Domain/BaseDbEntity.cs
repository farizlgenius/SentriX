using System;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Domain;

public class BaseDbEntity
{
      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public short component_id { get; set; }
      public int location_id { get; set; }
      public DateTime created_at { get; set; }
      public DateTime updated_at { get; set; }
      public bool is_active { get; set; } = true;
      public bool is_default { get; set; } = false;

      public BaseDbEntity()
      {

      }

      public BaseDbEntity(Guid guid, short componetId, int locationId, bool isactive, bool isdefault)
      {
            this.guid = guid;
            this.component_id = componetId;
            this.location_id = locationId;
            this.is_active = isactive;
            this.is_default = isdefault;
      }

      public void Disable()
      {
            this.is_active = false;
            this.updated_at = DateTime.UtcNow;
      }

      public void Enable()
      {
            this.is_active = true;
            this.updated_at = DateTime.UtcNow;
      }

}
