using Core.Contract.DTOs.Action;

namespace Core.Contract.DTOs.Procedure;

public sealed record ProcedureDto(
      Guid Guid,
      string Name,
      IEnumerable<ActionDto> Actions,
      Guid DeviceGuid,
      string DeviceName,
      bool IsActive,
      bool IsDefault
);
