using Core.Application.Interfaces;
using Core.Contract.DTOs.Output;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Output;

public sealed class OutputByDeviceQueryHandler(IControlRepository repo) : IQueryHandler<OutputByDeviceQuery, IEnumerable<ControlDto>>
{
      public async Task<IEnumerable<ControlDto>> HandleAsync(OutputByDeviceQuery query, CancellationToken ct)
      {
            return await repo.GetByDeviceAsync(query.guid, ct);
      }
}