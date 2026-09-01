using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record LocationIdsByGuidsQuery(IEnumerable<Guid> Guids) : IQuery<IEnumerable<int>>;