using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record InputDto(
      int Id,
      short ComponentId,
      string Name,
      string Mac,
      short DeviceComponentId,
      short ModuleComponentId,
      short InputNo,
      string Metadata,
      string Type,
      int LocationId,
      bool IsActive
) : BaseDto(ComponentId,LocationId,Type,IsActive);