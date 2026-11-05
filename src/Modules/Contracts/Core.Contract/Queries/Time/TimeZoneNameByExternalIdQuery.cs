using SharedKernel.Messaging;

namespace Core.Contract.Queries.Time;

public sealed record TimeZoneNameByExternalIdQuery(short externalId) : IQuery<string>;