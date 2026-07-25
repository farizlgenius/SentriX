using System.ComponentModel.DataAnnotations;

namespace Adapter.Aero.Model;

public class BaseSlot
{
      [Key]
      public int id { get; set; }
      public int slot_id { get; set; }
      public Guid? device_guid { get; set; }
      public Guid? component_guid { get; set; }
      public bool is_available { get; set; } = true;
      public DateTime created_at { get; set; }
      public DateTime updated_at { get; set; }

      public BaseSlot() { }

      public BaseSlot(int component_id)
      {
            this.slot_id = component_id;
            this.is_available = true;
      }

      public BaseSlot(Guid componentGuid, int component_id)
      {
            this.component_guid = componentGuid;
            this.slot_id = component_id;
            this.is_available = true;
      }

      public BaseSlot(int component_id, Guid scpGuid)
      {
            this.device_guid = scpGuid;
            this.slot_id = component_id;
            this.is_available = true;
      }

      public BaseSlot(Guid scpGuid, Guid componentGuid, int component_id)
      {
            this.device_guid = scpGuid;
            this.component_guid = componentGuid;
            this.slot_id = component_id;
            this.is_available = true;
      }

      public void Inserted(Guid component)
      {
            this.component_guid = component;
            this.is_available = false;
            this.updated_at = DateTime.UtcNow;
      }

      public void Ejected()
      {
            this.component_guid = null;
            this.is_available = true;
            this.updated_at = DateTime.UtcNow;
      }
}