using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record DeviceDto(
      int Id=0,
      string Name="",
      short ComponentId=0,
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
      bool IsActive=true
) : BaseDto(ComponentId,LocationId,Type,IsActive);