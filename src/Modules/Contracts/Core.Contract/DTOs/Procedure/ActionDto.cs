namespace Core.Contract.DTOs.Procedure;

public sealed record ActionDto(
      Guid Guid,
      Dictionary<short,string> Detail
);