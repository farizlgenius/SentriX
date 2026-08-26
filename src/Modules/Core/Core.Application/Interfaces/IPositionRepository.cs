using Core.Contract.DTOs.Position;
using Core.Contract.DTOs.User;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IPositionRepository : IBaseRepository<PositionDto, Position>
{
      Task<bool> IsAnyUserAsync(Guid guid, CancellationToken ct = default);
      Task<Pagination<PositionDto>> GetPaginationByDepartmentGuidAsync(PaginationParams param, Guid guid, CancellationToken ct = default);
      Task<bool> IsAnyNameByDepartmentGuidAsync(string name, Guid guid, CancellationToken ct = default);


}