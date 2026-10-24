using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class BreakGlass : BaseDomain
{
  public int SlotNo { get; private set; }
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public string Metadata {get; private set;} = string.Empty;
  public int DeviceModuleId { get; private set; }
  public BreakGlass(
    int slotNo,
    Vendor vendor,
    string metadata,
    int deviceModuleId,
    short externalId
  ) : base(Guid.NewGuid(),externalId)
  {
    SlotNo = slotNo;
    Vendor = vendor;
    Metadata = metadata;
    DeviceModuleId = deviceModuleId;
  }

  public BreakGlass(
    Guid guid,
    int slotNo,
    Vendor vendor,
     string metadata,
    int deviceModuleId,
     short externalId
  ) : base(guid,externalId)
  {
    SlotNo = slotNo;
    Vendor = vendor;
    Metadata = metadata;
    DeviceModuleId = deviceModuleId;
  }
}