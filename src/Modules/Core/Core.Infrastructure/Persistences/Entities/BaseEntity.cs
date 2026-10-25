using System.ComponentModel.DataAnnotations;
using Core.Application.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public class BaseEntity : IBaseEntity
{
  [Key]
  public int id { get; set; }
  public string name {get; set;} = string.Empty;
  public Guid guid { get; set; }
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }
  public bool is_active { get; set; } = true;
  public bool is_default { get; set; } = false;
  public int? location_id {get; set; }
  public Location? location {get; set;}
  public short? external_id {get; set; } 
  public short? altr_external_id { get; set; }


  public BaseEntity()
  {

  }

  public BaseEntity(Guid guid,short externalId = -1,short altrExternalId=-1)
  {
    this.guid = guid;
    created_at = DateTime.UtcNow;
    updated_at = DateTime.UtcNow;
    if(externalId != -1)
      external_id = externalId;
    if(altrExternalId != -1)
      altr_external_id = altrExternalId;
  }


}