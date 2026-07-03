using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record NameAndLocationByMacQuery(string Mac) : IQuery<(string Name,int LocationId)>;