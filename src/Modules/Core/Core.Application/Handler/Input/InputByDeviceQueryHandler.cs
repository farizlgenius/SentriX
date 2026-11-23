using Core.Application.Interfaces;
using Core.Contract.DTOs.Monitor;
using Core.Contract.Queries.Input;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Input;

public sealed class InputByDeviceQueryHandler(IMonitorRepository repo) : IQueryHandler<InputByDeviceQuery, IEnumerable<MonitorDto>>
{
      public async Task<IEnumerable<MonitorDto>> HandleAsync(InputByDeviceQuery query, CancellationToken ct)
      {
           return await repo.GetByDeviceAsync(query.guid, ct);
      }
}