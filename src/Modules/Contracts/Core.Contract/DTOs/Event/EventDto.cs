using SharedKernel.Enums;

namespace Core.Contract.DTOs.Event;

public sealed record EventDto(
  Guid Guid,
  DateTime Timestamp,
  string Actor,
  string Module,
  string EventType,
  string ImageName,
  string Mac,
  string ComponentName,
  string EventCode,
  string Remarks,
  string CaptureImageName,
  Vendor Vendor,
  Guid? LocationGuid,
  bool IsActive,
  bool IsDefault
);