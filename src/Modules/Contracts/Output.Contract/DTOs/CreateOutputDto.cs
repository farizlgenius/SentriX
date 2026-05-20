using SharedKernel.Domain;

namespace Output.Contract.DTOs;

public sealed record CreateOutputDto(
      string Name,
      string Mac,
      short DeviceComponentId,
      short ModuleComponentId,
      short OutputNo,
      string Model,
      short RelayMode,
      int LocationId,
      short DefaultPulse,
      string Type,
      bool IsActive
      ) : BaseDto(0,LocationId,Type,IsActive);