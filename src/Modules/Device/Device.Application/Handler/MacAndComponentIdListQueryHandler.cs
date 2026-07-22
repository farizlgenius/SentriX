using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class GuidAndTypeByLocationIdQueryHandler(IDeviceRepository repo) : IQueryHandler<GuidAndTypeByLocationIdQuery,IEnumerable<(Guid Guid,string Type)>>
{
      public async Task<IEnumerable<(Guid Guid,string Type)>> HandleAsync(GuidAndTypeByLocationIdQuery query, CancellationToken ct)
      {
            return await repo.GetGuidAndTypesByLocationIdAsync(query.LocationId);
      }
}