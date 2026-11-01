using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Contract.Queries.Input;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Input;

public sealed class InputCommandDetailByGuidQueryHandler(IInputRepository repo) : IQueryHandler<InputCommandDetailByGuidQuery, CommandDetailDto>
{
      public async Task<CommandDetailDto> HandleAsync(InputCommandDetailByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetCommandDetailAsync(query.guid,ct);
      }
}