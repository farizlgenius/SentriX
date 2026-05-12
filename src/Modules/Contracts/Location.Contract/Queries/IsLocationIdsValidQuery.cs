using SharedKernel.Messaging;

namespace Location.Contract.Queries;

public sealed record IsLocationIdsValidQuery(List<int> LocationIds) : IQuery<bool>;