using Group.Contract.DTOs;
using SharedKernel.Messaging;

namespace Group.Contract.Queries;

public sealed record GroupsListByRangeIdQuery(
      List<int> Ids
      ) : IQuery<IEnumerable<GroupSplitByMacDto>>;