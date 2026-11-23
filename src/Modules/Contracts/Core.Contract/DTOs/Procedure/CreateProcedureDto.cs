using Core.Contract.DTOs.Action;

namespace Core.Contract.DTOs.Procedure;

public sealed record CreateProcedureDto(
      string Name,
      List<CreateActionDto> Actions,
      Guid DeviceGuid,
      bool IsActive,
      bool IsDefault
);