using System;
using Operator.Application.Interfaces;
using Operator.Contract.Queries;
using SharedKernel.Messaging;

namespace Operator.Application.Queries;


public sealed class LocationListByUsernameQueryHandler(IOperatorRepository repo) : IQueryHandler<LocationListByUsernameQuery, List<int>>
{
      public async Task<List<int>> HandleAsync(LocationListByUsernameQuery query, CancellationToken ct)
      {
            return await repo.GetLocationIdsByUsernameAsync(query.Username, ct);
      }
}
