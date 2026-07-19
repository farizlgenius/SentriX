using SharedKernel.Domain;
using User.Contract.DTOs;
using User.Domain.Entities;

namespace User.Application.Interfaces;

public interface ICompanyRepository
{
      Task<CompanyDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task AddAsync(Company dto,CancellationToken ct = default);
      Task<bool> IsAnyNameAsync(string name, CancellationToken ct = default);
      Task<bool> IsAnyByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<string> CheckRelateRecordAsync(Guid guid,CancellationToken ct = default);
      Task UpdateImagePathAsync(string path,Guid guid,CancellationToken ct = default);
      Task DeleteAsync(Guid guid, CancellationToken ct = default);
      Task<Pagination<CompanyDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task UpdateAsync(Domain.Entities.Company company,CancellationToken ct = default);  
      Task<IEnumerable<CompanyDto>> GetByLocationIdAsync(int LocationId,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(int locationId,CancellationToken ct = default);
      Task<bool> IsAnyRelateAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetCompanyOptionByLocationIdAsync(int location,CancellationToken ct = default);

}