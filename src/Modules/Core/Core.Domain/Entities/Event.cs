using SharedKernel.Constants;

namespace Core.Domain.Entities;

public sealed class Event : BaseDomain
{
  public DateTime Timestampe { get; set; }
  public string Actor { get; set; } = string.Empty;
  public string Module { get; set; } = string.Empty;
  public string EventType { get; set; } = string.Empty;
  public string ImageName { get; set; } = string.Empty;
  public string Mac { get; set; } = string.Empty;
  public string ComponentName { get; set; } = string.Empty;
  public string EventCode { get; set; } = string.Empty;
  public string Remarks { get; set; } = string.Empty;
  public string CaptureImageName { get; set; } = string.Empty;
  public SharedKernel.Enums.Vendor Vendor { get; set; } = SharedKernel.Enums.Vendor.aero;
  public int LocationId { get; set; }

  public Event(
    DateTime timeStamp,
    string actor,
    string module,
    string eventType,
    string imageName,
    string mac,
    string componentName,
    string eventCode,
    string remarks,
    string captureImageName
  ) : base(Guid.NewGuid())
  {
    Timestampe = timeStamp;
    Actor = actor;
    Module = module;
    EventType = eventType;
    ImageName = imageName;
    Mac = mac;
    ComponentName = componentName;
    EventCode = eventCode;
    Remarks = remarks;
    CaptureImageName = captureImageName;
  }
}