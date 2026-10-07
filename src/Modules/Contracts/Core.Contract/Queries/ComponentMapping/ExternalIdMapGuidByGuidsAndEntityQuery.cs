using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record ExternalIdMapGuidByGuidsAndEntityQuery(IEnumerable<Guid> guids,EntityType entity) : IQuery<Dictionary<Guid,int>>;