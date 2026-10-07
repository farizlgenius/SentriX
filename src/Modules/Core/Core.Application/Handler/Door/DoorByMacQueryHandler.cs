using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Contract.Queries.Door;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Door;

public sealed class DoorByMacQueryHandler(IDoorRepository repo) : IQueryHandler<DoorByMacQuery, IEnumerable<DoorDto>>
{
      public async Task<IEnumerable<DoorDto>> HandleAsync(DoorByMacQuery query, CancellationToken ct)
      {
            return await repo.GetByMacAsync(query.mac,ct);
      }
}