using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record InputDto(
      int Id=0,
      short ComponentId=0,
      string Name="",
      string Mac="",
      short DeviceComponentId=0,
      short ModuleComponentId=0,
      short InputNo=0,
      short SensorMode=0,
      short Debounce=0,
      short HoldTime=0,
      short LogFunction=0,
      short LatchMode=0,
      short DelayEntry=0,
      short DelayExit=0,
      string Type="",
      int LocationId=0,
      bool IsActive=false
) : BaseDto(ComponentId,LocationId,Type,IsActive);