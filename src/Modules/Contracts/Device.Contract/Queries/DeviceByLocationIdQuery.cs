using Device.Contract.DTOs;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record DeviceByLocationIdQuery(int LocationId) : IQuery<IEnumerable<DeviceDto>>;