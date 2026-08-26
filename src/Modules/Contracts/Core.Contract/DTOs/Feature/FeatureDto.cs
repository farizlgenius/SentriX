namespace Core.Contract.DTOs.Feature;

public sealed record FeatureDto(
      Guid Guid,
      string Name,
      string Module
);