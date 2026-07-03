using System;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Domain;

public class BaseEntity
{
  [Key]
  public int id { get; set; }
  public short component_id {get; set;}
  public int location_id {get; set;}
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }
  public bool is_active { get; set; } = true;
  public bool is_default {get; set;} = false;

  public BaseEntity()
  {
    
  }

  public BaseEntity(short componetId,int locationId,bool isactive,bool isdefault)
  {
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
