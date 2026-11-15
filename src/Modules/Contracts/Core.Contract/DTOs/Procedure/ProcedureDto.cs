using Core.Contract.DTOs.Action;

namespace Core.Contract.DTOs.Procedure;

public sealed record ProcedureDto(
      Guid Guid,
      string Name,
      List<ActionProcedureDto> Actions,
      Guid DeviceGuid,
      string DeviceName,
      bool IsActive,
      bool IsDefault
);

public sealed record ActionProcedureDto(
      Guid Guid,
      string Name,
      int Sequence
);