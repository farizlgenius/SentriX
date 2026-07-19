using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record CreateModuleDto(
      string Mac,
      short Model,
      short Port,
      short Address,
      Guid DeviceGuid,
      int LocationId,
      string Type,
      bool IsActive,
      bool IsDefault
) : BaseDtoEntity(Guid.Empty,LocationId,Type,IsActive,IsDefault);

