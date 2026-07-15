using System.ComponentModel.DataAnnotations;

public class AmicoDbEntity
{
  [Key]
  public int id { get; set; }
  public Guid guid {get; set;}
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }

  public AmicoDbEntity()
  {
    
  }

  public AmicoDbEntity(Guid guid)
  {
    this.guid = guid;
  }


}
