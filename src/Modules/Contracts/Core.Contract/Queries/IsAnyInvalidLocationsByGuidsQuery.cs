using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record IsAnyInvalidLocationsByGuidsQuery(List<Guid> LocationGuids) : IQuery<IEnumerable<Guid>>;