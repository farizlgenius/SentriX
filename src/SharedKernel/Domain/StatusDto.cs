namespace SharedKernel.Domain;

public sealed record StatusDto(
      int Id,
      int ComponentId,
      string Status,
      string Tamper,
      string Ac,
      string Batt
);

