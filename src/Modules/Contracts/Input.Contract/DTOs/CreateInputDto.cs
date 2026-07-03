using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record CreateInputDto(
      string Name,
      string Mac,
      short DeviceComponentId,
      short ModuleComponentId,
      short InputNo,
      short SensorMode,
      short Debounce,
      short HoldTime,
      short LogFunction,
      short LatchMode,
      short DelayEntry,
      short DelayExit,
      string Type,
      int LocationId,
      bool IsActive
) : BaseDto(0,LocationId,Type,IsActive);