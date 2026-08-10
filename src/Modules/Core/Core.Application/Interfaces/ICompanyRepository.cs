using Core.Contract.DTOs.Company;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface ICompanyRepository : IBaseRepository<CompanyDto, Company>
{
  Task<bool> IsAnyDepartmentAsync(Guid guid, CancellationToken ct = default);
  Task<bool> IsAnyUserAsync(Guid guid, CancellationToken ct = default);
}