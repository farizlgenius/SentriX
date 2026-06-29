using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record ModuleDto(
      int Id = 0,
      short ComponentId=0,
      string Name="",
      string Fw="",
      string SerialNumber="",
      short Port=0,
      short Address=0,
      string Mac="",
      string Model="",
      string Type="",
      int DeviceComponentId=0,
      int LocationId=0,
      bool IsActive=true
      ) : BaseDto(ComponentId,LocationId,Type,IsActive);