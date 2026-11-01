using Core.Application.Interfaces;
using Core.Contract.DTOs.Input;
using Core.Contract.Queries.Input;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Input;

public sealed class InputByDeviceQueryHandler(IInputRepository repo) : IQueryHandler<InputByDeviceQuery, IEnumerable<InputDto>>
{
      public async Task<IEnumerable<InputDto>> HandleAsync(InputByDeviceQuery query, CancellationToken ct)
      {
           return await repo.GetByDeviceAsync(query.guid, ct);
      }
}