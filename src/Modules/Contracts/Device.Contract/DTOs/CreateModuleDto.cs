using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record CreateModuleDto(
      short ComponentId,
      string Mac,
      short Model,
      short Port,
      short Address,
      int DeviceComponentId,
      Guid DeviceGuid,
      int LocationId,
      string Type,
      bool IsActive,
      bool IsDefault
) : BaseDtoEntity(Guid.Empty,ComponentId,LocationId,Type,IsActive,IsDefault);

