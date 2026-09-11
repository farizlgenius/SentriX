using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Reader : BaseDomain
{
  public int SlotNo { get; private set; }
  public ReaderMode ReaderMode { get; private set; }
  public string Metadata { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public ReaderDirection ReaderDirection { get; private set; } = ReaderDirection.In;
  public int DeviceModuleId { get; private set; }
  public Reader(
    int slotNo,
    ReaderMode readerMode,
    string metadata,
    Vendor vendor,
    ReaderDirection readerDirection,
    int deviceModuleId
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    ReaderMode = readerMode;
    Metadata = metadata;
    Vendor = vendor;
    ReaderDirection = readerDirection;
    DeviceModuleId = deviceModuleId;
  }

  public Reader(
    Guid guid,
    int slotNo,
    ReaderMode readerMode,
    string metadata,
    Vendor vendor,
    ReaderDirection readerDirection,
    int deviceModuleId
  ) : base(guid)
  {
    SlotNo = slotNo;
    ReaderMode = readerMode;
    Metadata = metadata;
    Vendor = vendor;
    ReaderDirection = readerDirection;
    DeviceModuleId = deviceModuleId;
  }
}