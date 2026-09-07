using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Door : BaseDomain
{
  public string Name { get; set; } = string.Empty;
  public Vendor Vendor { get; set; }
  public DoorType Type { get; set; }
  public DoorDirection Direction { get; set; }
  public string Metadta { get; set; } = string.Empty;
  public Door(
    string name,
    Vendor vendor,
    DoorType type,
    DoorDirection direction,
    string metadata,


  ) : base(Guid.NewGuid())
  { }
  public Door(
    Guid guid
  ) : base(guid)
  { }
}