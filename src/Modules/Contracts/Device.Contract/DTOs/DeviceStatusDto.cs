using System;

namespace Device.Contract.DTOs;

public sealed record DeviceStatusDto(Guid guid,bool Status);
