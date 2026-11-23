using SharedKernel.Enums;

namespace Core.Contract.DTOs.Action;

public sealed record ActionTargetDto(
      Guid Guid,
      string Name
);