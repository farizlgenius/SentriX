using Core.Contract.DTOs.Company;

namespace Core.Contract.Interfaces;

public interface ICompany : IService<CompanyDto, CreateCompanyDto, UpdateCompanyDto>
{
  Task<IEnumerable<CompanyDto>> GetAsync();
}