using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record CreateAlertDto(
      EntityType Entity,
      string Detail
);