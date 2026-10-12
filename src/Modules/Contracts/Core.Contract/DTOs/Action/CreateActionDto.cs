namespace Core.Contract.DTOs.Action;

public sealed record CreateActionDto(
      string Name,
      string Metadata
);