using Core.Application.Interfaces;
using Core.Contract.Queries.Door;
using Core.Contract.Queries.MonitorGroup;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Application.Handler.MonitorGroup;

public sealed class ComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorQueryHandler(IMonitorGroupRepository repo) : IQueryHandler<ComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorQuery, IEnumerable<(Guid Guid,short ExternalId,MonitorType Type)>>
{
      public async Task<IEnumerable<(Guid Guid,short ExternalId,MonitorType Type)>> HandleAsync(ComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorAsync(query.externalId,query.deviceExternalId,query.vendor,ct);
      }
}