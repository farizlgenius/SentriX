using Core.Contract.DTOs.Action;

namespace Core.Contract.DTOs.Procedure;

public sealed record UpdateProcedureDto(
      Guid Guid,
     string Name,
      List<UpdateActionDto> Actions,
      Guid DeviceGuid,
      bool IsActive,
      bool IsDefault
);