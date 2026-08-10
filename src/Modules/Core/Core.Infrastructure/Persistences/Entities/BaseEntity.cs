using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public class BaseEntity
{
  [Key]
  public int id { get; set; }
  public Guid guid { get; set; }
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }
  public bool is_active { get; set; } = true;
  public bool is_default { get; set; } = false;


  public BaseEntity() { }

  public BaseEntity(Guid guid)
  {
    this.guid = guid;
  }


}