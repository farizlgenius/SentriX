using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Buzzer : BaseDomain
{
  public int SlotNo { get; private set; }
  public OutputMode OutputMode { get; private set; }
  public string Metadata { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public Buzzer(
    int slotNo,
    OutputMode outputMode,
    string metadata,
    Vendor vendor
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    OutputMode = outputMode;
    Metadata = metadata;
    Vendor = vendor;
  }

  public Buzzer(
    Guid guid,
    int slotNo,
    OutputMode outputMode,
    string metadata,
    Vendor vendor
  ) : base(guid)
  {
    SlotNo = slotNo;
    OutputMode = outputMode;
    Metadata = metadata;
    Vendor = vendor;
  }
}