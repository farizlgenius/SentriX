namespace Core.Contract.DTOs.Procedure;

public sealed record UpdateProcedureDto(
      Guid Guid,
      string Name,
     List<(Guid Guid,int Sequence)> Actions,
      Guid DeviceGuid
);