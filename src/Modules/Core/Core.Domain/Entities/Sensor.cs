using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Sensor : BaseDomain
{
  public int SlotNo { get; private set; }
  public InputMode InputMode { get; private set; }
  public string Metadata { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public Sensor(
    int slotNo,
    InputMode inputMode,
    string metadata,
    Vendor vendor
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    InputMode = inputMode;
    Metadata = metadata;
    Vendor = vendor;
  }

  public Sensor(
    Guid guid,
    int slotNo,
    InputMode inputMode,
    string metadata,
    Vendor vendor
  ) : base(guid)
  {
    SlotNo = slotNo;
    InputMode = inputMode;
    Metadata = metadata;
    Vendor = vendor;
  }
}