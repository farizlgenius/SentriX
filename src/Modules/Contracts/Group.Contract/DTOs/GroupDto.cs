using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record GroupDto(
      Guid Guid=default,
      string Name="",
      List<GroupDoorDto> Doors=default!, 
      int LocationId=0, 
      bool IsActive=false,
      bool IsDefault=false
      ) : BaseDtoEntity(Guid, LocationId, string.Empty, IsActive,IsDefault);

