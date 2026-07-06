using Input.Application.Interfaces;
using Input.Contract.DTOs;
using Input.Contract.Queries;
using SharedKernel.Messaging;

namespace Input.Application.Handler;

public sealed class InputByMacQueryHandler(IInputRepository repo) : IQueryHandler<InputByMacQuery, IEnumerable<InputDto>>
{
      public async Task<IEnumerable<InputDto>> HandleAsync(InputByMacQuery query, CancellationToken ct)
      {
            return await repo.GetInputByMacAsync(query.Mac,ct);
      }
}