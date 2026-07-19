using Group.Contract.DTOs;
using SharedKernel.Messaging;

namespace Group.Contract.Queries;

public sealed record GroupsListByRangeGuidQuery(
      List<Guid> Guids
      ) : IQuery<IEnumerable<GroupSplitByMacDto>>;