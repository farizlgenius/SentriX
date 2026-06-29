using SharedKernel.Messaging;
using Time.Contract.DTOs;

namespace Time.Contract.Queries;

public sealed record TimeZoneByLocationIdQuery(int LocationId) : IQuery<IEnumerable<TimezoneDto>>;