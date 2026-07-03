using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record GroupDto(
      int Id=0,
      short ComponentId=0,
      string Name="",
      List<GroupDoorDto> Doors=default!, 
      int LocationId=0, 
      bool IsActive=false,
      bool IsDefault=false
      ) : BaseDto(ComponentId, LocationId, string.Empty, IsActive);

