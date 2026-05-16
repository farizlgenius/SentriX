using System;
using Device.Contract.DTOs;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record DeviceByMacAsyncQuery(string Mac) : IQuery<DeviceDto>
;