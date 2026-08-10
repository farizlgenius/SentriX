using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Country : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public string code { get; set; } = string.Empty;
  public ICollection<Location> locations { get; set; } = default!;
  public Country() { }
  
}