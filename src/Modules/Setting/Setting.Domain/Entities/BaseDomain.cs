namespace Setting.Domain.Entities;

public class BaseDomain
{
  public Guid Guid { get; set; }

  public BaseDomain(Guid Guid)
  {
    this.Guid = Guid;
  }

  public BaseDomain()
  {
    this.Guid = Guid.NewGuid();
  }
}