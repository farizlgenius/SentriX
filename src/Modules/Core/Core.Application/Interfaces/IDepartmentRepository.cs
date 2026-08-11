using Core.Contract.DTOs.Department;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IDepartmentRepository : IBaseRepository<DepartmentDto, Department>
{
      Task<bool> IsAnyPositionAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyUserAsync(Guid guid,CancellationToken ct = default);
      Task<Pagination<DepartmentDto>> GetPaginationByCompanyGuidAsync(PaginationParams param,Guid companyGuid,CancellationToken ct = default);
      Task<bool> IsAnyNameByCompanyGuidAsync(string name,Guid guid,CancellationToken ct = default);
}