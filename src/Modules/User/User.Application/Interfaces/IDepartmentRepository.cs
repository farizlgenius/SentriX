using SharedKernel.Domain;
using User.Contract.DTOs;
using User.Domain.Entities;

namespace User.Application.Interfaces;

public interface IDepartmentRepository
{
      Task<bool> IsAnyByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyNameAsync(Guid companyGuid,string name,CancellationToken ct = default);
      Task AddAsync(Department domain,CancellationToken ct = default);
      Task<DepartmentDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyRelateAsync(Guid guid,CancellationToken ct = default);
      Task DeleteAsync(Guid guid,CancellationToken ct = default);
      Task<Pagination<DepartmentDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task UpdateAsync(Department domain,CancellationToken ct = default);

      Task<Pagination<DepartmentDto>> GetPaginationByCompanyGuidAsync(PaginationParams param,Guid guid,CancellationToken ct = default);
      Task<IEnumerable<DepartmentDto>> GetDepartmentByCompanyGuidAsync(Guid companyGuid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetDepartmentOptionByCompanyGuidAsync(Guid guid,CancellationToken ct = default);
}