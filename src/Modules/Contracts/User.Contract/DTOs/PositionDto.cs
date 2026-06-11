using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record PositionDto(
      int Id,
      string Name,
      string Description,
      int LocationId,
      bool IsActive
) : BaseDto(
      0,
      LocationId,
      string.Empty,
      IsActive
);