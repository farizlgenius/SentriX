using SharedKernel.Messaging;

namespace Input.Contract.Queries;

public sealed record IsAnyInputGroupNotSyncQuery(string Mac,int LocationId,DateTime SyncAt) : IQuery<bool>;