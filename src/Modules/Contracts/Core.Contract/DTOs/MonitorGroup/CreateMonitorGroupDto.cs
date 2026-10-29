namespace Core.Contract.DTOs.MonitorGroup;

public sealed record CreateMonitorGroupDto(
      string Name,
      List<MonitorGroupComponentDto> Components,
      Guid DeviceGuid
);