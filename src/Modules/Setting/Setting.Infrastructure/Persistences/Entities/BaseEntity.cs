using System.ComponentModel.DataAnnotations;

namespace Setting.Infrastructure.Persistences.Entities;

public class BaseEntity
{
  [Key]
  public int id { get; set; }
  public Guid guid { get; set; }
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }
  public BaseEntity() { }
  public BaseEntity(Guid guid)
  {
    this.guid = guid;
  }
}