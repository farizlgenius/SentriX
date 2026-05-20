using System;
using SharedKernel.Domain;

namespace Device.Contract.DTOs;

public sealed record DeviceDto(
      int Id,
      string Name,
      short ComponentId,
      string SerialNumber,
      string Mac,
      string Ip,
      int Port,
      string Fw,
      string Type,
      string Status,
      DateTime SyncedAt,
      int LocationId,
      string Metadata,
      bool IsActive
) : BaseDto(ComponentId,LocationId,Type,IsActive);