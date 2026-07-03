using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record IsAnyModuleNotSyncQuery(string Mac,int LocationId,DateTime SyncAt) : IQuery<bool>;