using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record LocationIdByGuidQuery(Guid Guid) : IQuery<int>;