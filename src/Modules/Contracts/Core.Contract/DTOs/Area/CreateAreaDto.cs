using SharedKernel.Enums;

namespace Core.Contract.DTOs.Area;

public sealed record CreateAreaDto(
      string Name,
      AreaMultiOccMode MultiMode,
      AreaMode Mode,
      short OccMax,
      short OccDown,
      short OccUp,
      string Metadata,
      List<DoorAreaDto> Doors,
      Guid LocationGuid
);