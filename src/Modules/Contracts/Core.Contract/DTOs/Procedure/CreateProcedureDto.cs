namespace Core.Contract.DTOs.Procedure;

public sealed record CreateProcedureDto(
      string Name,
      List<Guid> Actions,
      Guid DeviceGuid,
      bool IsActive,
      bool IsDefault
);