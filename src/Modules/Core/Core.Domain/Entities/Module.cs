namespace Core.Domain.Entities;

public sealed class Module : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public Module(
    string name
  )
  {
    Name = name;
  }
}