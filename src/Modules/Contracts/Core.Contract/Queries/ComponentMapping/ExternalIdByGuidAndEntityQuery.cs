using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record ExternalIdByGuidAndEntityQuery(Guid guid,EntityType entity) : IQuery<int>;