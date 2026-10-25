using Core.Application.Interfaces;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Output;

public sealed class OutputExternalIdByGuidQueryHandler(IOutputRepository repo) : IQueryHandler<OutputExternalIdByGuidQuery, short>
{
      public async Task<short> HandleAsync(OutputExternalIdByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdByGuidAsync(query.guid,ct);
      }
}