using System;

namespace Device.Contract.DTOs;

public sealed record CreateDeviceDto(
      int ComponentId,
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
);
