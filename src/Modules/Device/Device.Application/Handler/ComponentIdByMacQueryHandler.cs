using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class ComponentIdByMacQueryHandler(IDeviceRepository repo) : IQueryHandler<ComponentIdByMacQuery, int>
{
      public async Task<int> HandleAsync(ComponentIdByMacQuery query, CancellationToken ct)
      {
            return await repo.GetComponentIdByMacAsync(query.Mac);
      }
}