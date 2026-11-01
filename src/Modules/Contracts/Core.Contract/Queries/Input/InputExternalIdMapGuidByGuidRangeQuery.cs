using SharedKernel.Messaging;

namespace Core.Contract.Queries.Input;

public sealed record InputExternalIdMapGuidByGuidRangeQuery(IEnumerable<Guid> guids) : IQuery<Dictionary<Guid,short>>;