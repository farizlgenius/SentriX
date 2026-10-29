using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Relay : BaseDomain
{
  public short SlotNo { get; private set; }
  public string Metadata { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int DeviceModuleId { get; private set; }
  public Relay(
    short slotNo,
    string metadata,
    Vendor vendor,
    int deviceModuleId
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    Metadata = metadata;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }

  public Relay(
    Guid guid,
    short slotNo,
    string metadata,
    Vendor vendor,
    int deviceModuleId
  ) : base(guid)
  {
    SlotNo = slotNo;
    Metadata = metadata;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }
}