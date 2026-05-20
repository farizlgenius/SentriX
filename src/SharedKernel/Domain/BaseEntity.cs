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

  public BaseEntity()
  {
    
  }

  public BaseEntity(short componetId,int locationId,bool isactive)
  {
    this.component_id = componetId;
    this.location_id = locationId;
    this.is_active = isactive;
  }

}
