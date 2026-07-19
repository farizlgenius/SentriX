using SharedKernel.Domain;

namespace Door.Contract.DTOs;

public sealed record CreateDoorDto(
      Guid Guid,
      string Name,
      short DeviceComponentId,
      string Mac,
      string DoorType,
      string Metadata,
      int LocationId,
      string Type,
      bool IsActive,
      bool IsDefault
      ) : BaseDtoEntity(Guid,LocationId,Type,IsActive,IsDefault);