using SharedKernel.Messaging;

namespace Core.Contract.Queries.Time;

public sealed record TimeZoneIdsMapGuidsByGuidsQuery(IEnumerable<Guid> Guids) : IQuery<Dictionary<Guid, int>>;