using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class CompanyIdByGuidQueryHandler(
  ICompanyRepository repo
) : IQueryHandler<CompanyIdByGuidQuery, int>
{
  public async Task<int> HandleAsync(CompanyIdByGuidQuery query, CancellationToken ct)
  {
    return await repo.GetIdByGuidAsync(query.guid, ct);
  }
}