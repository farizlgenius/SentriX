using SharedKernel.Messaging;

namespace Setting.Contract.Queries;

public sealed record IsAnyCardFormatNotSyncQuery(int LocationId,DateTime SyncAt) : IQuery<bool>;