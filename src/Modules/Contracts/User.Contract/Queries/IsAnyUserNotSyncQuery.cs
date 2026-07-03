using SharedKernel.Messaging;

namespace User.Contract.Queries;

public sealed record IsAnyUserNotSyncQuery(int LocationId,DateTime SyncAt) : IQuery<bool>;