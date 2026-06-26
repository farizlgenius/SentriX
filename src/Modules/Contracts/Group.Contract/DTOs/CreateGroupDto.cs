using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record CreateGroupDto(
      string Name,
      List<GroupDoorDto> Doors,
      int LocationId, 
      bool IsActive) : BaseDto(0, LocationId, string.Empty, IsActive);