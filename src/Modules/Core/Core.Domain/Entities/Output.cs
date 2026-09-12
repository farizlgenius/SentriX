using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Output : BaseDomain
{
  public string Name { get; set; } = string.Empty;
  public int SlotNo { get; set; }
  public string Metadata { get; set; } = string.Empty;
  public OutputMode Mode { get; set; } = OutputMode.NC;
  public Vendor Vendor { get; set; } = Vendor.aero;
  public int DeviceModuleId { get; set; }
  public int LocationId { get; set; }

  public Output(
    string name,
    int slotNo,
    string metadata,
    OutputMode mode,
    Vendor vendor,
    int deviceModuleId,
    int locationId
  ) : base(Guid.NewGuid())
  {
    ValidationHelper.Name(name);
    ValidationHelper.Vendor(vendor);
    Name = name;
    SlotNo = slotNo;
    Metadata = metadata;
    Mode = mode;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
    LocationId = locationId;
  }

  public Output(
    Guid guid,
    string name,
    int slotNo,
    string metadata,
    OutputMode mode,
    Vendor vendor,
    int deviceModuleId,
    int locationId
  ) : base(guid)
  {
    ValidationHelper.Name(name);
    ValidationHelper.Vendor(vendor);
    Name = name;
    SlotNo = slotNo;
    Metadata = metadata;
    Mode = mode;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
    LocationId = locationId;
  }
}