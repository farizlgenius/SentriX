using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record InputGroupDto(
      int Id = 0,
      string Name = "",
      List<InputGroupDetailDto> InputGroupDetailDtos = default!,
      short ComponentId = 0,
      int LocationId = 0,
      string Type = "",
      bool IsActive = false
) : BaseDto(
      ComponentId,LocationId,Type,IsActive
);