using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Vendor
{
  [Key]
  public int id { get; set; }
  public string name { get; set; } = string.Empty;
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }

  public Vendor() { }
}