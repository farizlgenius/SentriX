using System;

namespace Device.Contract.DTOs;

public sealed record ModuleDto(
      int Id,
      string Name,
      string Fw,
      string SerialNumber,
      int Port,
      int Address,
      string Mac,
      string Model,
      int DeviceId);