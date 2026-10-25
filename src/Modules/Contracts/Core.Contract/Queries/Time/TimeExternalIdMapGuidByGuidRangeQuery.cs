using SharedKernel.Messaging;

namespace Core.Contract.Queries.Time;

public sealed record TimeExternalIdMapGuidByGuidRangeQuery(IEnumerable<Guid> guids) : IQuery<Dictionary<Guid,short>>;