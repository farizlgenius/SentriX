using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record ExternalIdByMacAndEntityQuery(string mac,EntityType entity) : IQuery<int>;