using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class MacAndComponentIdListQueryHandler(IDeviceRepository repo) : IQueryHandler<MacAndComponentIdListByLocationIdQuery, IEnumerable<(string Mac, short ComponentId)>>
{
      public async Task<IEnumerable<(string Mac, short ComponentId)>> HandleAsync(MacAndComponentIdListByLocationIdQuery query, CancellationToken ct)
      {
            return await repo.MacAndComponentIdListAsync(query.LocationId);
      }
}