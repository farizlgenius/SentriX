using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Input : BaseDomain
{
  public string Name { get; set; } = string.Empty;
  public int SlotNo { get; set; }
  public string Metadata { get; set; } = string.Empty;
  public Vendor Vendor { get; set; } = Vendor.aero;
  public int DeviceModuleId { get; set; }
  public int LocationId { get; set; }

  public Input(
    string name,
    int slotNo,
    string metadata,
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
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
    LocationId = locationId;
  }

  public Input(
    Guid guid,
    string name,
    int slotNo,
    string metadata,
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
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
    LocationId = locationId;
  }
}