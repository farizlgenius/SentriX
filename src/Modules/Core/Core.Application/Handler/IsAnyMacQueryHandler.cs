using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class IsAnyMacQueryHandler(IDeviceRepository repo) : IQueryHandler<IsAnyMacQuery, bool>
{
  public async Task<bool> HandleAsync(IsAnyMacQuery query, CancellationToken ct)
  {
    return await repo.IsAnyMacAsync(query.mac, ct);
  }
}