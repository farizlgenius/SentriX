using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Buzzer : BaseDomain
{
  public short SlotNo { get; private set; }
  public string Metadata { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int DeviceModuleId { get; private set; }
  public Buzzer(
    short slotNo,
    string metadata,
    Vendor vendor,
    int deviceModuleId,
    short externalId 
  ) : base(Guid.NewGuid(),externalId)
  {
    SlotNo = slotNo;
    Metadata = metadata;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }

  public Buzzer(
    Guid guid,
    short slotNo,
    string metadata,
    Vendor vendor,
    int deviceModuleId,
    short externalId
  ) : base(guid,externalId)
  {
    SlotNo = slotNo;
    Metadata = metadata;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }
}