using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record GroupIdsByGuidsQuery(IEnumerable<Guid> Guids) : IQuery<IEnumerable<int>>;