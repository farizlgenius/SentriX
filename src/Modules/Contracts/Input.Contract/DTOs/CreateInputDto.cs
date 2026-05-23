using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record CreateInputDto(
      string Name,
      string Mac,
      short DeviceComponentId,
      short ModuleComponentId,
      short InputNo,
      string Metadata,
      string Type,
      int LocationId,
      bool IsActive
) : BaseDto(0,LocationId,Type,IsActive);