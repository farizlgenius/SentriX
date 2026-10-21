using Core.Contract.DTOs.Company;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface ICompanyRepository : IRepository<CompanyDto, Company>
{
  Task<bool> IsAnyUserAsync(Guid guid, CancellationToken ct = default);
  Task<IEnumerable<CompanyDto>> GetAsync();
}