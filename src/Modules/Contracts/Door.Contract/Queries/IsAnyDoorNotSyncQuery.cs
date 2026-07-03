using SharedKernel.Messaging;

namespace Door.Contract.Queries;

public sealed record IsAnyDoorNotSyncQuery(string Mac,int LocationId,DateTime SyncAt) : IQuery<bool>;