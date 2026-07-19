using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Domain;

public class BaseSlot
{
      [Key]
      public int id { get; set; }
      public int slot_id { get; set; }
      public Guid device_guid { get; set; }
      public Guid? component_guid { get; set; }
      public bool is_available { get; set; } = true;
      public DateTime created_at { get; set; } 
      public DateTime updated_at { get; set; }

      public BaseSlot(){}

      public BaseSlot(Guid scpGuid,int component_id)
      {
            this.device_guid = scpGuid;
            this.slot_id = component_id;
            this.is_available = is_available;
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