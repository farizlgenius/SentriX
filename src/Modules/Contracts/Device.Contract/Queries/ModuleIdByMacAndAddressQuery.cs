using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record ModuleIdByMacAndAddressQuery(string Mac,int Address) : IQuery<int>;