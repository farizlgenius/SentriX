using Core.Application.Interfaces;
using Core.Contract.DTOs.Output;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Output;

public sealed class OutputByDeviceQueryHandler(IOutputRepository repo) : IQueryHandler<OutputByDeviceQuery, IEnumerable<OutputDto>>
{
      public async Task<IEnumerable<OutputDto>> HandleAsync(OutputByDeviceQuery query, CancellationToken ct)
      {
            return await repo.GetByDeviceAsync(query.guid, ct);
      }
}