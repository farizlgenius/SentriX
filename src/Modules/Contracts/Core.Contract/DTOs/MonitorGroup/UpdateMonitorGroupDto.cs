namespace Core.Contract.DTOs.MonitorGroup;

public sealed record UpdateMonitorGroupDto(
      Guid Guid,
      string Name,
      List<MonitorGroupComponentDto> Components,
      Guid DeviceGuid
);