using Core.Contract.DTOs.MonitorGroup;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.MonitorGroup;

public sealed record MonitorGroupByDeviceQuery(Guid guid) : IQuery<IEnumerable<MonitorGroupDto>>;