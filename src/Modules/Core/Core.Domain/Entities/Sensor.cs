using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Sensor : BaseDomain
{
  public int SlotNo { get; private set; }
  public InputMode InputMode { get; private set; }
  public string Metadata { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int DeviceModuleId { get; private set; }
  public Sensor(
    int slotNo,
    InputMode inputMode,
    string metadata,
    Vendor vendor,
    int deviceModuleId
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    InputMode = inputMode;
    Metadata = metadata;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }

  public Sensor(
    Guid guid,
    int slotNo,
    InputMode inputMode,
    string metadata,
    Vendor vendor,
    int deviceModuleId
  ) : base(guid)
  {
    SlotNo = slotNo;
    InputMode = inputMode;
    Metadata = metadata;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }
}