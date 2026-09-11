using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Relay : BaseDomain
{
  public int SlotNo { get; private set; }
  public OutputMode OutputMode { get; private set; }
  public string Metadata { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int DeviceModuleId { get; private set; }
  public Relay(
    int slotNo,
    OutputMode outputMode,
    string metadata,
    Vendor vendor,
    int deviceModuleId
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    OutputMode = outputMode;
    Metadata = metadata;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }

  public Relay(
    Guid guid,
    int slotNo,
    OutputMode outputMode,
    string metadata,
    Vendor vendor,
    int deviceModuleId
  ) : base(guid)
  {
    SlotNo = slotNo;
    OutputMode = outputMode;
    Metadata = metadata;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }
}