using Door.Application.Interfaces;
using Door.Contract.DTOs;
using Door.Contract.Queries;
using SharedKernel.Messaging;

namespace Door.Application.Handler;

public sealed class DoorByMacQueryHandler(IDoorRepository repo) : IQueryHandler<DoorByMacQuery, IEnumerable<DoorDto>>
{
      public async Task<IEnumerable<DoorDto>> HandleAsync(DoorByMacQuery query, CancellationToken ct)
      {
            return await repo.GetDoorByMacAsync(query.Mac);
      }
}