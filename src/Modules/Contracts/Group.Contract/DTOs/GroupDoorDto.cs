using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record GroupDoorDto(
      string Mac,
      short DeviceComponentId,
      short DoorComponentId,
      short TimezoneComponentId,
      string Type
      );
