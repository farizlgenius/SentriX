using Core.Contract.DTOs.Time;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Time;

public sealed record IntervalByGuidsQuery(IEnumerable<Guid> guids) : IQuery<IEnumerable<IntervalDto>>;