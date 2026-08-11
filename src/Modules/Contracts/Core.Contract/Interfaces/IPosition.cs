using Core.Contract.DTOs.Position;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IPosition : IBase<PositionDto,CreatePositionDto,UpdatePositionDto>
{
      Task<Pagination<PositionDto>> GetPaginationByDepartmentGuidAsync(PaginationParams param,Guid departmentGuid,CancellationToken ct = default);
}