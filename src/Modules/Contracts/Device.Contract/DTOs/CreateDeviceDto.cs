using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record CreateDeviceDto(
      short ComponentId,
      string Name,
      string SerialNumber,
      string Mac,
      string Ip,
      int Port,
      string Fw,
      string Type,
      string Status,
      DateTime SyncedAt,
      int LocationId,
      string Metadata
) : BaseDto(ComponentId,LocationId,Type,true);
