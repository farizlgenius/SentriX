using SharedKernel.Domain;
using User.Contract.DTOs;

namespace User.Contract.Interfaces;

public interface ICompany
{
            // Company
      Task<Pagination<CompanyDto>> GetCompanyPaginationAsync(PaginationParams param);
      Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto);
      Task<CompanyDto> UpdateCompanyAsync(CompanyDto dto);
      Task<CompanyDto> DeleteCompanyAsync(int id);
      Task<IEnumerable<CompanyDto>> GetCompanyByLocationIdAsync(int LocationId);
      Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationAsync(int LocationId);
}
