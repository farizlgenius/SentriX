using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Reader : BaseDomain
{
  public int SlotNo { get; set; }
  public ReaderMode ReaderMode { get; set; }
  public string Metadata { get; set; } = string.Empty;
  public int DeviceModuleId { get; set; }
  public Reader(
    int slotNo,
    ReaderMode readerMode,
    string metadata,
    int deviceModuleId
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    ReaderMode = readerMode;
    Metadata = metadata;
    DeviceModuleId = deviceModuleId;
  }

  public Reader(
    Guid guid,
    int slotNo,
    ReaderMode readerMode,
    string metadata,
    int deviceModuleId
  ) : base(guid)
  {
    SlotNo = slotNo;
    ReaderMode = readerMode;
    Metadata = metadata;
    DeviceModuleId = deviceModuleId;
  }
}