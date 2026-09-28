using SharedKernel.Messaging;

namespace Core.Contract.Queries.Time;

public sealed record IntervalIdsByGuidsQuery(IEnumerable<Guid> guids) : IQuery<IEnumerable<int>>;