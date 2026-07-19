using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record CreateGroupDto(
      string Name,
      List<GroupDoorDto> Doors,
      int LocationId, 
      bool IsActive,
      bool IsDefault
      ) : BaseDtoEntity(Guid.Empty, LocationId, string.Empty, IsActive,IsDefault);