using Core.Contract.DTOs.Action;

namespace Core.Contract.DTOs.Procedure;

public sealed record ProcedureDto(
      Guid Guid,
      string Name,
      List<ActionDto> Actions,
      string LocationName,
      string DeviceName,
      bool IsActive,
      bool IsDefault
);