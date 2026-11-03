using Core.Application.Interfaces;
using Core.Contract.Queries.MonitorGroup;
using SharedKernel.Messaging;

namespace Core.Application.Handler.MonitorGroup;

public sealed class MonitorGroupGuidByExternalIdAndDeviceExternalIdAndVendorQueryHandler(IMonitorGroupRepository repo) : IQueryHandler<MonitorGroupGuidByExternalIdAndDeviceExternalIdAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(MonitorGroupGuidByExternalIdAndDeviceExternalIdAndVendorQuery query, CancellationToken ct)
      {
           return await repo.GetGuidByExternalIdAndDeviceExternalIdAndVendorAsync(query.externalId,query.deviceExternalId,query.vendor,ct);
      }
}