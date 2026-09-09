namespace Core.Domain.Entities;

public sealed class Turnstile : BaseDomain
{
  public string Name { get; set; } = string.Empty;
  public List<Lane> Lanes { get; set; } = default!;
  public int LocationId { get; set; }
  public Turnstile(
    string name,
    List<Lane> lanes,
    int locationId
  ) : base(Guid.NewGuid())
  {
    Name = name;
    Lanes = lanes;
    LocationId = locationId;
  }

  public Turnstile(
    Guid Guid,
    string name,
    List<Lane> lanes,
    int locationId
  ) : base(Guid)
  {
    Name = name;
    Lanes = lanes;
    LocationId = locationId;
  }

}