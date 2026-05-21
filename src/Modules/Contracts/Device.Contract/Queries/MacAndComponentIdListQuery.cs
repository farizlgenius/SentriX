using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record MacAndComponentIdListByLocationIdQuery(int LocationId) : IQuery<IEnumerable<(string Mac,short ComponentId,string Type)>>;