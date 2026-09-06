using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record IntervalIdsByGuidsQuery(IEnumerable<Guid> guids) : IQuery<IEnumerable<int>>;