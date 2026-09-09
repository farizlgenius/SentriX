using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Relay : BaseDomain
{
  public int SlotNo { get; private set; }
  public OutputMode OutputMode { get; private set; }
  public string Metadata { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int doorId { get; private set; }
  public Relay(
    int slotNo,
    OutputMode outputMode,
    string metadata,
    Vendor vendor,
    int doorId
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    OutputMode = outputMode;
    Metadata = metadata;
    Vendor = vendor;
    this.doorId = doorId;
  }

  public Relay(
    Guid guid,
    int slotNo,
    OutputMode outputMode,
    string metadata,
    Vendor vendor,
    int doorId
  ) : base(guid)
  {
    SlotNo = slotNo;
    OutputMode = outputMode;
    Metadata = metadata;
    Vendor = vendor;
    this.doorId = doorId;
  }
}