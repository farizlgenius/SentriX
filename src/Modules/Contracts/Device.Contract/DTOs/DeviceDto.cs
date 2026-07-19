using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record DeviceDto(
      Guid Guid=default,
      string Name="",
      string SerialNumber="",
      string Mac="",
      string Ip="",
      int Port=0,
      string Fw="",
      string Type="",
      string Status="",
      DateTime? SyncedAt=null,
      int LocationId=0,
      string Metadata="",
      bool IsActive=true,
      bool IsDefault=false
) : BaseDtoEntity(Guid,LocationId,Type,IsActive,IsDefault);