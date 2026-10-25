using SharedKernel.Messaging;

namespace Core.Contract.Queries.Door;

public sealed record DoorExternalIdMapGuidByGuidRangeQuery(IEnumerable<Guid> guids) : IQuery<Dictionary<Guid,short>>;