using SharedKernel.Domain;

namespace Door.Contract.DTOs;

public sealed record CreateDoorDto(
      short ComponentId,
      string Name,
      short DeviceComponentId,
      string Mac,
      string DoorType,
      string Metadata,
      int LocationId,
      string Type,
      bool IsActive
      ) : BaseDto(ComponentId,LocationId,Type,IsActive);