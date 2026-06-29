using Group.Contract.DTOs;
using SharedKernel.Messaging;

namespace Group.Contract.Queries;

public sealed record GroupByMacAndDeviceTypeQuery(string Mac,string Type) : IQuery<IEnumerable<GroupDto>>;