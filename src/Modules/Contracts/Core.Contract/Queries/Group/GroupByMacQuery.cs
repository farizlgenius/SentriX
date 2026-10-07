using Core.Contract.DTOs.Door;
using Core.Contract.DTOs.Group;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Group;

public sealed record GroupByMacQuery(string mac) : IQuery<IEnumerable<GroupDto>>;