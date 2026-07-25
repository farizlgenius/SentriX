using System.ComponentModel.DataAnnotations;

namespace Adapter.Amico.Persistences.Entities;

public class BaseSlot
{
      [Key]
      public int id { get; set; }
      public Guid? guid { get; set; }
      public Guid? device_guid { get; set; }
      public int slot_id { get; set; }
      public DateTime created_at { get; set; }
      public DateTime updated_at { get; set; }

      public BaseSlot(Guid guid, int slot)
      {
            this.guid = guid;
            this.slot_id = slot;
      }

      public BaseSlot(Guid guid, Guid device_guid, int slot)
      {
            this.guid = guid;
            this.device_guid = device_guid;
            this.slot_id = slot;
      }

}