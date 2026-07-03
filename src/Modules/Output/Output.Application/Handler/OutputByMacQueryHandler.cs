using Output.Application.Interfaces;
using Output.Contract.DTOs;
using Output.Contract.Queries;
using SharedKernel.Messaging;

namespace Output.Application.Handler;

public sealed class OutputByMacQueryHandler(IOutputRepository repo) : IQueryHandler<OutputByMacQuery, IEnumerable<OutputDto>>
{
      public async Task<IEnumerable<OutputDto>> HandleAsync(OutputByMacQuery query, CancellationToken ct)
      {
            return await repo.GetByMacAsync(query.Mac,ct);
      }
}