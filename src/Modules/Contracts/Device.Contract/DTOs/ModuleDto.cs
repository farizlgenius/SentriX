using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record ModuleDto(
      Guid Guid = default,
      string Name="",
      string Fw="",
      string SerialNumber="",
      short Port=0,
      short Address=0,
      string Mac="",
      string Model="",
      string Type="",
      Guid DeviceGuid=default,
      int LocationId=0,
      bool IsActive=true,
      bool IsDefault=false
      ) : BaseDtoEntity(Guid,LocationId,Type,IsActive,IsDefault);