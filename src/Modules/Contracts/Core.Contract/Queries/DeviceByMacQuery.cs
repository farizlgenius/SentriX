using Core.Contract.DTOs.Device;
using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record DeviceByMacQuery(string mac) : IQuery<DeviceDto>;