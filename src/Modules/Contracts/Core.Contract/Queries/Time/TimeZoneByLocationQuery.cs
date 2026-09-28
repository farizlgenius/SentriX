using Core.Contract.DTOs.Time;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Time;

public sealed record TimeZoneByLocationQuery(Guid guid) : IQuery<IEnumerable<TimeZoneDto>>;