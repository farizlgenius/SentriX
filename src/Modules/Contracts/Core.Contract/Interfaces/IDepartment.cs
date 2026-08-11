using Core.Contract.DTOs.Department;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IDepartment : IBase<DepartmentDto,CreateDepartmentDto,UpdateDepartmentDto>
{
      Task<Pagination<DepartmentDto>> GetPaginationByCompanyGuidAsync(PaginationParams param,Guid companyGuid,CancellationToken ct = default);
}