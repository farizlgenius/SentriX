using SharedKernel.Messaging;

namespace Core.Contract.Queries.Door;

public sealed record DoorExternalIdRangeByGuidQuery(Guid guid) : IQuery<IEnumerable<short>>;