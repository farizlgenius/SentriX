using Group.Contract.DTOs;
using SharedKernel.Messaging;

namespace Group.Contract.Queries;

public sealed record GroupIdListByMacQuery(
      string Mac
      ) : IQuery<IEnumerable<(int id,short componentId)>>;