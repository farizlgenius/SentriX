using Device.Contract.DTOs;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record DeviceByComponentIdQuery(int ScpId) : IQuery<DeviceDto>;