using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Domain;

public class CentralBaseSlot
{
      [Key]
      public int id { get; set; }
      public int slot_id { get; set; }
      public Guid? component_guid { get; set; }
      public bool is_available { get; set; } = true;
      public DateTime created_at { get; set; } 
      public DateTime updated_at { get; set; }

      public CentralBaseSlot(){}

      public CentralBaseSlot(int component_id)
      {
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