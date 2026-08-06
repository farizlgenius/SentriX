using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Country
{
  [Key]
  public int id { get; set; }
  public string name { get; set; } = string.Empty;
  public string code { get; set; } = string.Empty;
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }
  public ICollection<Location> locations { get; set; } = default!;
  public Country() { }

}