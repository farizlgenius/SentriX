

using Core.Application.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Constants;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class ExternalIdMapGuidByGuidsAndEntityQueryHandler(IComponentMappingRepository repo) : IQueryHandler<ExternalIdMapGuidByGuidsAndEntityQuery, Dictionary<Guid,int>>
{
      public async Task<Dictionary<Guid,int>> HandleAsync(ExternalIdMapGuidByGuidsAndEntityQuery query, CancellationToken ct)
      {
            var res = await repo.GetExternalIdMapGuidByGuidsAndEntityAsync(query.guids,query.entity);

            return res;
      }
}