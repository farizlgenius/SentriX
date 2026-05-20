namespace SharedKernel.Domain;

public sealed record StatusDto(
      int DeviceComponentId,
      int ComponentId,
      string Status,
      string Tamper,
      string Ac,
      string Batt
);

