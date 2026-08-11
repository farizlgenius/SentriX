namespace Core.Infrastructure.Persistences.Entities;

public sealed class OperatorLocation : BaseEntity
{
  public Guid operator_guid { get; set; }
  public Operator @operator { get; set; } = default!;
  public Guid location_guid { get; set; }
  public Location location { get; set; } = default!;

  public OperatorLocation() { }
  public OperatorLocation(
    Guid oper,
    Guid loc
  ) : base(Guid.NewGuid())
  {
    operator_guid = oper;
    location_guid = loc;
  }
}