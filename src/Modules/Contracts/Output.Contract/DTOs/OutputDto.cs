using System;
using SharedKernel.Domain;

namespace Output.Contract.DTOs;

public sealed record OutputDto(
      int Id = 0,
      string Name = "",
      string Mac = "",
      short ComponentId=0,
      short DeviceComponentId=0,
      short ModuleComponentId=0,
      short OutputNo=0,
      string Model="",
      short OfflineMode=0,
      short DriveMode=0,
      int LocationId=0,
      short DefaultPulse=0,
      string Type="",
      bool IsActive=false
     ) : BaseDto(ComponentId,LocationId,Type,IsActive); 
