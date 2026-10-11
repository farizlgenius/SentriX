namespace Core.Contract.DTOs.Procedure;

public sealed record ProcedureDto(
      Guid Guid,
      string Name,
      List<ActionDto> Actions,
      bool IsActive,
      bool IsDefault
);