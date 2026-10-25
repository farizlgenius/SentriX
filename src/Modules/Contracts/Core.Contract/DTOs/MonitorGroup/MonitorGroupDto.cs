namespace Core.Contract.DTOs.MonitorGroup;

public sealed record MonitorGroupDto(
      Guid Guid,
      string Name,
      bool IsActive,
      bool IsDefault
);