using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record CoreEntityDto(
      Guid Guid,
      EntityType Entity
);