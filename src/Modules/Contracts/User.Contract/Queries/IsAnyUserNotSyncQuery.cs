using SharedKernel.Messaging;

namespace User.Contract.Queries;

public sealed record IsAnyUserNotSyncQuery(string Mac,int LocationId,DateTime SyncAt) : IQuery<bool>;