using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record ModuleDto(
      int Id,
      short ComponentId,
      string Name,
      string Fw,
      string SerialNumber,
      short Port,
      short Address,
      string Mac,
      string Model,
      string Type,
      int DeviceComponentId,
      int LocationId,
      bool IsActive
      ) : BaseDto(ComponentId,LocationId,Type,IsActive);