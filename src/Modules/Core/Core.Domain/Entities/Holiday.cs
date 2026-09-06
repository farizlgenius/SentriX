using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Holiday : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public DateTime Start { get; private set; }
  public DateTime End { get; private set; }
  public int LocationId { get; private set; }
  public Holiday(
    string name,
    DateTime start,
    DateTime end,
    int locationId
  ) : base(Guid.NewGuid())
  {
    ValidationHelper.Name(name);
    ValidationHelper.ValidateActiveTime(start, end);
    Name = name;
    Start = start;
    End = end;
    LocationId = locationId;
  }

  public Holiday(
    Guid guid,
    string name,
    DateTime start,
    DateTime end,
    int locationId
  ) : base(guid)
  {
    ValidationHelper.Name(name);
    ValidationHelper.ValidateActiveTime(start, end);
    Name = name;
    Start = start;
    End = end;
    LocationId = locationId;
  }
}