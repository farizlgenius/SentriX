namespace Core.Contract.DTOs.Action;

public sealed record UpdateActionDto(
      Guid Guid,
      string Name,
      string Metadata
);