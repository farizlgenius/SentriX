using Core.Application.Interfaces;
using Core.Contract.Queries.MonitorGroup;
using SharedKernel.Messaging;

namespace Core.Application.Handler.MonitorGroup;

public sealed class MonitorGroupGuidByExternalIdAndMacAndVendorQueryHandler(IMonitorGroupRepository repo) : IQueryHandler<MonitorGroupGuidByExternalIdAndMacAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(MonitorGroupGuidByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
           return await repo.GetGuidByExternalIdAndMacAndVendorAsync(query.externalId,query.mac,query.vendor,ct);
      }
}