using Output.Application.Interfaces;
using Output.Contract.DTOs;
using Output.Contract.Queries;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Output.Application.Handler;

public sealed class OutputByIdQueryHandler(IOutputRepository repo) : IQueryHandler<OutputByIdQuery, OutputDto>
{
      public async Task<OutputDto> HandleAsync(OutputByIdQuery query, CancellationToken ct)
      {
            var res = await repo.GetByGuidAsync(query.Id,ct);
            if(res.Id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Output",query.Id));

            return res;
      }
}