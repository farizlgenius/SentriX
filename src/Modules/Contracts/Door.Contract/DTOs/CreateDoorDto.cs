using SharedKernel.Domain;

namespace Door.Contract.DTOs;

public sealed record CreateDoorDto(
      string Name,
      string Mac,
      string DoorType,
      string Metadata,
      int LocationId,
      string Type,
      bool IsActive,
      bool IsDefault
      ) : BaseDtoEntity(Guid.Empty,LocationId,Type,IsActive,IsDefault);