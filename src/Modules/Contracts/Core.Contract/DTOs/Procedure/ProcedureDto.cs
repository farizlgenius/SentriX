namespace Core.Contract.DTOs.Procedure;

public sealed record ProcedureDto(
      Guid Guid,
      string Name,
      string Metadata,
      bool IsActive,
      bool IsDefault
);