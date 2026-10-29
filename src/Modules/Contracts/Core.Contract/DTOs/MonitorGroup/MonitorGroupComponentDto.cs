using SharedKernel.Enums;

namespace Core.Contract.DTOs.MonitorGroup;

public sealed record MonitorGroupComponentDto(
      Guid ComponentGuid,
      MonitorType Type
);