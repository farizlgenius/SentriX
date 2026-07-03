using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record CreateInputGroupDto(
      int Id,
      string Name,
      List<InputGroupDetailDto> InputGroupDetailDtos,
      int LocationId,
      string Type,
      bool IsActive
) : BaseDto(
      0,LocationId,Type,IsActive
);