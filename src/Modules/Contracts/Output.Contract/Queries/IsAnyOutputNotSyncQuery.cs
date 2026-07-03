using SharedKernel.Messaging;

namespace Output.Contract.Queries;

public sealed record IsAnyOutputNotSyncQuery(string Mac,int LocationId,DateTime SyncAt) : IQuery<bool>;