namespace Core.Contract.DTOs.Role;

public sealed record ModuleDto(
      Guid Guid,
      string Name,
      IEnumerable<FeatureDto> Features
);