namespace Core.Domain.Entities;

public class BaseDomain
{
  public Guid Guid { get; private set; }

  public BaseDomain()
  {
    this.Guid = Guid.NewGuid();
  }

  public BaseDomain(
    Guid Guid
  )
  {
    this.Guid = Guid;
  }

}