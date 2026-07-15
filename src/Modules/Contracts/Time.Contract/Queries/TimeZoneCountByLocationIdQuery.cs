using SharedKernel.Messaging;

namespace Time.Contract.Queries;

public sealed record TimeZoneCountByLocationIdQuery(int LocationId) : IQuery<int>;