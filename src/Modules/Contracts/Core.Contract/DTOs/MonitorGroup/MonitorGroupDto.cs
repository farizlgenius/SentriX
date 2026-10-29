namespace Core.Contract.DTOs.MonitorGroup;

public sealed record MonitorGroupDto(
      Guid Guid,
      string Name,
      List<MonitorGroupComponentDto> Components,
      Guid DeviceGuid,
      bool IsActive,
      bool IsDefault
);