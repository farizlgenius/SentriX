using SharedKernel.Enums;

namespace SharedKernel.Model.Status;


public sealed record MonitorGroupStatusDto(
      Guid Guid,
      short MaskCount,
      short NumActive,
      MonitorGroupComponentStatusDto[] ActiveList
);


public sealed record MonitorGroupComponentStatusDto(
      MonitorType Type,
      Guid Guid
);
