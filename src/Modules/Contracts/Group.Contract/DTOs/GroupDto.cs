using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record GroupDto(int Id,short ComponentId,string Name,List<GroupDoorDto> Doors, int LocationId, bool IsActive) : BaseDto(ComponentId, LocationId, string.Empty, IsActive);

