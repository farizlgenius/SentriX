using SharedKernel.Domain;

namespace Door.Contract.DTOs;

public sealed record DoorDto(
      int Id,
       short ComponentId,
      string Name,
      short DeviceComponentId,
      short SecondComponentId,
      string Mac,
      string DoorType,
      string Metadata,
      int LocationId,
      string Type,
      bool IsActive
      ) : BaseDto(ComponentId,LocationId,Type,IsActive);