using Core.Contract.DTOs.Monitor;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Input;

public sealed record InputByDeviceQuery(Guid guid) : IQuery<IEnumerable<MonitorDto>>;