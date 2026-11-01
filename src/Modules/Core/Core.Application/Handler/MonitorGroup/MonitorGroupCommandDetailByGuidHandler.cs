using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Contract.Queries.Input;
using Core.Contract.Queries.MonitorGroup;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.MonitorGroup;

public sealed class MonitorGroupCommandDetailByGuidQueryHandler(IMonitorGroupRepository repo) : IQueryHandler<MonitorGroupCommandDetailByGuidQuery, CommandDetailDto>
{
      public async Task<CommandDetailDto> HandleAsync(MonitorGroupCommandDetailByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetCommandDetailAsync(query.guid,ct);
      }
}