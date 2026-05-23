using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record InputGroupDto(
      int Id,
      string Name,
      string Mac,
      short DeviceComponentId,
      string Metadata,
      short ComponentId,
      int LocationId,
      string Type,
      bool IsActive
) : BaseDto(
      ComponentId,LocationId,Type,IsActive
);