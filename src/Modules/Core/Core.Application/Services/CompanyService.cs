using Core.Contract.DTOs.Company;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class CompanyService() : ICompany
{
  public Task<CompanyDto> CreateAsync(CreateCompanyDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<Guid> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<CompanyDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<Pagination<CompanyDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task<CompanyDto> UpdateAsync(UpdateCompanyDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}