using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record PositionIdByGuidQuery(Guid guid) : IQuery<int>;