using SharedKernel.Domain;

namespace Door.Contract.DTOs;

public sealed record DoorDto(
      Guid Guid=default,
       short ComponentId=0,
      string Name="",
      short DeviceComponentId=0,
      short SecondComponentId=0,
      string Mac="",
      string DoorType="",
      string Metadata="",
      int LocationId=0,
      string Type="",
      bool IsActive=true,
      bool IsDefault=false
      ) : BaseDtoEntity(Guid,ComponentId,LocationId,Type,IsActive,IsDefault);