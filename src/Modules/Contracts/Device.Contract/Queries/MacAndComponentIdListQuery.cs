using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record GuidAndTypeByLocationIdQuery(int LocationId) : IQuery<IEnumerable<(Guid Guid,string Type)>>;