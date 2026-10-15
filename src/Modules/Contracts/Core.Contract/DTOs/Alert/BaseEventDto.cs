using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record BaseEventDto(
      Guid Guid,
      EntityType Type,
      string Detail
);