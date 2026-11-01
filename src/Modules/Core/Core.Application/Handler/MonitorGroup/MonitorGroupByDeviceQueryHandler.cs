using Core.Application.Interfaces;
using Core.Contract.DTOs.MonitorGroup;
using Core.Contract.Queries.MonitorGroup;
using SharedKernel.Messaging;

namespace Core.Application.Handler.MonitorGroup;

public sealed class MonitorGroupByDeviceQueryHandler(IMonitorGroupRepository repo) : IQueryHandler<MonitorGroupByDeviceQuery, IEnumerable<MonitorGroupDto>>
{
      public async Task<IEnumerable<MonitorGroupDto>> HandleAsync(MonitorGroupByDeviceQuery query, CancellationToken ct)
      {
            return await repo.GetByDeviceAsync(query.guid,ct);
      }
}