using SharedKernel.Domain;
using User.Contract.DTOs;
using User.Domain.Entities;

namespace User.Application.Interfaces;

public interface IPositionRepository
{
      Task<bool> IsAnyByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyNameAsync(Guid departmentGuid,string name,CancellationToken ct= default);
      Task AddAsync(Position domain,CancellationToken ct = default);
      Task DeleteAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyRelateAsync(Guid guid,CancellationToken ct = default);
      Task<PositionDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<Pagination<PositionDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task UpdateAsync(Position domain,CancellationToken ct = default);
      Task<Pagination<PositionDto>> GetPositionPaginationByDepartmentGuidAsync(PaginationParams param,Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetPositionOptionByDepartmentGuidAsync(Guid guid,CancellationToken ct = default);
}