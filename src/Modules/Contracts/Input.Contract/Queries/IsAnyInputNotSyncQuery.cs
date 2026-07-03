using SharedKernel.Messaging;

namespace Input.Contract.Queries;

public sealed record IsAnyInputNotSyncQuery(string Mac,int LocationId,DateTime SyncAt) : IQuery<bool>;