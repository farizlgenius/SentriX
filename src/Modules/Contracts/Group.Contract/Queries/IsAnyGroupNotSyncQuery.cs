using SharedKernel.Messaging;

namespace Group.Contract.Queries;

public sealed record IsAnyGroupNotSyncQuery(int LocationId,DateTime SyncAt) : IQuery<bool>;