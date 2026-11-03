using Core.Application.Interfaces;
using Core.Contract.Queries.MonitorGroup;
using SharedKernel.Messaging;

namespace Core.Application.Handler.MonitorGroup;

public sealed class MonitorGroupNameByExternalIdAndMacAndVendorQueryHandler(IMonitorGroupRepository repo) : IQueryHandler<MonitorGroupNameByExternalIdAndMacAndVendorQuery, string>
{
      public async Task<string> HandleAsync(MonitorGroupNameByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetNameByExternalIdAndMacAndVendorAsync(query.ExternalId,query.Mac,query.Vendor,ct);
      }
}