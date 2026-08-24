namespace Core.Infrastructure.Persistences.Entities;

public sealed class Group : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public Group() { }
}