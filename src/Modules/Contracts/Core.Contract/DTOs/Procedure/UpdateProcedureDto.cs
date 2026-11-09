namespace Core.Contract.DTOs.Procedure;

public sealed record UpdateProcedureDto(
      Guid Guid,
      string Name,
      List<Guid> Actions,
      Guid DeviceGuid
);