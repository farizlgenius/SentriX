using Input.Application.Interfaces;
using Input.Contract.DTOs;
using Input.Contract.Queries;
using SharedKernel.Messaging;

namespace Input.Application.Handler;

public sealed class InputGroupByMacQueryHandler(IInputRepository repo) : IQueryHandler<InputGroupByMacQuery, IEnumerable<InputGroupDto>>
{
      public async Task<IEnumerable<InputGroupDto>> HandleAsync(InputGroupByMacQuery query, CancellationToken ct)
      {
            return await repo.GetInputGroupByMacAsync(query.Mac);
      }
}