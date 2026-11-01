using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Contract.Queries.Input;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Output;

public sealed class OutputCommandDetailByGuidQueryHandler(IOutputRepository repo) : IQueryHandler<OutputCommandDetailByGuidQuery, CommandDetailDto>
{
      public async Task<CommandDetailDto> HandleAsync(OutputCommandDetailByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetCommandDetailAsync(query.guid,ct);
      }
}