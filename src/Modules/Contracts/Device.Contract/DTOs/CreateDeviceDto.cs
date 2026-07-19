using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record CreateDeviceDto(
      string Name,
      string SerialNumber,
      short ScpId,
      string Mac,
      string Ip,
      int Port,
      string Fw,
      string Type,
      string Status,
      DateTime SyncedAt,
      int LocationId,
      string Metadata
) : BaseDtoEntity(Guid.Empty,LocationId,Type,true,true);
