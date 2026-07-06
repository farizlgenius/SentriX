using SharedKernel.Messaging;

namespace Group.Contract.Queries;

public sealed record MacsByGroupIdsQuery(IEnumerable<int> Ids) : IQuery<IEnumerable<string>>;