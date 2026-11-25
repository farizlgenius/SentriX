using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Monitor : BaseDomain
{
  public string Name { get; set; } = string.Empty;
  public short SlotNo { get; set; }
  public string Metadata { get; set; } = string.Empty;
  public int DeviceModuleId { get; set; }
  public int LocationId { get; set; }

  public Monitor(
    string name,
    short slotNo,
    string metadata,
    int deviceModuleId,
    int locationId,
    short externalId
  ) : base(Guid.NewGuid(),externalId)
  {
    ValidationHelper.Name(name);
    Name = name;
    SlotNo = slotNo;
    Metadata = metadata;
    DeviceModuleId = deviceModuleId;
    LocationId = locationId;
  }

  public Monitor(
    Guid guid,
    string name,
    short slotNo,
    string metadata,
    int deviceModuleId,
    int locationId,
    short externalId
  ) : base(guid,externalId)
  {
    ValidationHelper.Name(name);
    Name = name;
    SlotNo = slotNo;
    Metadata = metadata;
    DeviceModuleId = deviceModuleId;
    LocationId = locationId;
  }
}