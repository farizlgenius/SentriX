using Core.Contract.DTOs.Company;

namespace Core.Contract.Interfaces;

public interface ICompany : IBase<CompanyDto, CreateCompanyDto, UpdateCompanyDto>
{
  Task<IEnumerable<CompanyDto>> GetAsync();
}