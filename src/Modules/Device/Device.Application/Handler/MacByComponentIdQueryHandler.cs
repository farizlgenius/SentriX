using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class MacByComponentIdQueryHandler(IDeviceRepository repo) : IQueryHandler<MacByComponentIdQuery, string>
{
      public async Task<string> HandleAsync(MacByComponentIdQuery query, CancellationToken ct)
      {
            return await repo.GetMacByComponentIdAsync(query.ComponentId);
      }
}