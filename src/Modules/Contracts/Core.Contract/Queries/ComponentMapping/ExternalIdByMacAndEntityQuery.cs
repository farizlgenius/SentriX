using SharedKernel.Constants;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record ExternalIdByMacAndEntityQuery(string mac,string entity) : IQuery<int>;