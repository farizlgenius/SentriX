using SharedKernel.Constants;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record ExternalIdByGuidAndEntityQuery(Guid guid,string entity) : IQuery<int>;