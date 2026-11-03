namespace Core.Contract.DTOs.MonitorGroup;

public sealed record MonitorGroupDto(
      Guid Guid,
      string Name,
      List<MonitorGroupComponentDto> Components,
      Guid DeviceGuid,
      string DeviceName,
      bool IsActive,
      bool IsDefault
);