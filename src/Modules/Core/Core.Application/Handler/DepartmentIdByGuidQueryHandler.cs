using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class DepartmentIdByGuidQueryHandler(
  IDepartmentRepository repo
) : IQueryHandler<DepartmentIdByGuidQuery, int>
{
  public async Task<int> HandleAsync(DepartmentIdByGuidQuery query, CancellationToken ct)
  {
    return await repo.GetIdByGuidAsync(query.guid, ct);
  }
}