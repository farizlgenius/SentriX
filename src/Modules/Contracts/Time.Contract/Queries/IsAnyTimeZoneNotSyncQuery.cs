using SharedKernel.Messaging;

namespace Time.Contract.Queries;

public sealed record IsAnyTimeZoneNotSyncQuery(
      int LocationId,
      DateTime SyncAt
) : IQuery<bool>;