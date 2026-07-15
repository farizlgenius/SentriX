using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record GroupDto(
      Guid Guid=default,
      short ComponentId=0,
      string Name="",
      List<GroupDoorDto> Doors=default!, 
      int LocationId=0, 
      bool IsActive=false,
      bool IsDefault=false
      ) : BaseDtoEntity(Guid,ComponentId, LocationId, string.Empty, IsActive,IsDefault);

