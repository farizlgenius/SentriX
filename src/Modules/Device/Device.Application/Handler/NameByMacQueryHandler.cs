using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class NameByMacQueryHandler(IDeviceRepository repo) : IQueryHandler<NameAndLocationByMacQuery, (string Name,int LocationId)>
{

      public async Task<(string Name, int LocationId)> HandleAsync(NameAndLocationByMacQuery query, CancellationToken ct)
      {
            return await repo.GetNameAndLocationIdByMacAsync(query.Mac);
      }
}