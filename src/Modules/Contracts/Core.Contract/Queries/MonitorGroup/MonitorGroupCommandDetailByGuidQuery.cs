using Core.Contract.DTOs;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.MonitorGroup;

public sealed record MonitorGroupCommandDetailByGuidQuery(Guid guid) : IQuery<CommandDetailDto>;