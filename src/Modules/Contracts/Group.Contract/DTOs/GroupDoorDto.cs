using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record GroupDootDto(
      string Mac,
      short DeviceComponentId,
      short DoorComponentId,
      short TimezoneComponentId,
      string Type
      );
