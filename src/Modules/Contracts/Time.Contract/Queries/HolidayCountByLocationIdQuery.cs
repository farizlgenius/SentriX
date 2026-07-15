using SharedKernel.Messaging;

namespace Time.Contract.Queries;

public sealed record HolidayCountByLocationIdQuery(int LocationId) : IQuery<int>;