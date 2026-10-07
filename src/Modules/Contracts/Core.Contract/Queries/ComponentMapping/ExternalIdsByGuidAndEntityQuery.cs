using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record ExternalIdsByGuidAndEntityQuery(Guid guid,EntityType entity) : IQuery<IEnumerable<int>>;