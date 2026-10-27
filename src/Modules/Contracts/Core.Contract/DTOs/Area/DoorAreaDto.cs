using SharedKernel.Enums;

namespace Core.Contract.DTOs.Area;

public sealed record DoorAreaDto(
      Guid DoorGuid,
      AntipassBackMode AntipassbackFrom,
      Guid AreaFromGuid,
      AntipassBackMode AntipassbackTo,
      Guid? AreaToGuid,
      DoorType Type
);