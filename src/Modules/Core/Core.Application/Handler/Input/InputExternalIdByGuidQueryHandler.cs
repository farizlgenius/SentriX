using Core.Application.Interfaces;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Input;

public sealed class InputExternalIdByGuidQueryHandler(IInputRepository repo) : IQueryHandler<InputExternalIdByGuidQuery, short>
{
      public async Task<short> HandleAsync(InputExternalIdByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdByGuidAsync(query.guid,ct);
      }
}