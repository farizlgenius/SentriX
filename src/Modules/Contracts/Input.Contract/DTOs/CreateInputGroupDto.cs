using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record CreateInputGroupDto(
      string Name,
      string Mac,
      short DeviceComponentId,
      string Metadata,
      int LocationId,
      string Type,
      bool IsActive
) : BaseDto(
      0,LocationId,Type,IsActive
);