using System;
using SharedKernel.Domain;

namespace Output.Contract.DTOs;

public sealed record OutputDto(
      int Id,
      string Name,
      string Mac,
      short ComponentId,
      short DeviceComponentId,
      short ModuleComponentId,
      short OutputNo,
      string Model,
      short RelayMode,
      int LocationId,
      short DefaultPulse,
      string Type,
      bool IsActive
     ) : BaseDto(ComponentId,LocationId,Type,IsActive); 
