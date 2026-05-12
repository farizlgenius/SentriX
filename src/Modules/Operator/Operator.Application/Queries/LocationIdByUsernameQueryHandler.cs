using System;
using Operator.Application.Interfaces;
using Operator.Contract.Queries;
using SharedKernel.Messaging;

namespace Operator.Application.Queries;

public class LocationIdByUsernameQueryHandler(IOperatorRepository repo) : IQueryHandler<LocationIdByUsernameQuery, int>
{
      public async Task<int> HandleAsync(LocationIdByUsernameQuery query, CancellationToken ct)
      {
            return await repo.GetLocationIdByUsernameAsync(query.Username);
      }
}
