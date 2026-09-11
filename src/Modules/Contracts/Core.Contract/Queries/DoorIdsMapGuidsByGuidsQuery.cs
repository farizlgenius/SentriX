using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record DoorIdsMapGuidsByGuidsQuery(IEnumerable<Guid> Guids) : IQuery<Dictionary<Guid, int>>;