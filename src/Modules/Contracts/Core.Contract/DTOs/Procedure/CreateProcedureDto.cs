namespace Core.Contract.DTOs.Procedure;

public sealed record CreateProcedureDto(
      string Name,
      List<ActionDto> Actions
);