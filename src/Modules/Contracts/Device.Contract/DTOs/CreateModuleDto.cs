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
      int DeviceId,
      int LocationId,
      string Type,
      bool IsActive
) : BaseDto(ComponentId,LocationId,Type,IsActive);

