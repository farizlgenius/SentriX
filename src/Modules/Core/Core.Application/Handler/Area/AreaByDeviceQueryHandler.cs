using Core.Application.Interfaces;
using Core.Contract.DTOs.Area;
using Core.Contract.Queries.Area;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Area;

public sealed class AreaByDeviceQueryHandler(IAreaRepository repo) : IQueryHandler<AreaByDeviceQuery, IEnumerable<AreaDto>>
{
      public async Task<IEnumerable<AreaDto>> HandleAsync(AreaByDeviceQuery query, CancellationToken ct)
      {
            return await repo.GetByDeviceAsync(query.guid,ct);
      }
}