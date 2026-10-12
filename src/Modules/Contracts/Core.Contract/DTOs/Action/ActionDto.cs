namespace Core.Contract.DTOs.Action;

public sealed record ActionDto(
      Guid Guid,
      string Name,
      string Metadata,
      bool IsActive,
      bool IsDefault
);