using Door.Contract.DTOs;
using Door.Domain.Entities;
using SharedKernel.Domain;

namespace Door.Application.Interfaces;

public interface IDoorRepository
{
      Task<DoorDto> CreateAsync(Doors domain,CancellationToken ct = default);
      Task<short> GetLowestDoorComponentIdAsync(string Mac,CancellationToken ct = default);
      Task<short> GetLowestDoorComponentIdWithExceptionAsync(string Mac,List<int> Excepts,CancellationToken ct = default);
      Task<bool> IsAnyByIdAsync(int id,CancellationToken ct = default);
      Task<DoorDto> GetByIdAsync(int id,CancellationToken ct = default);
      Task<DoorDto> DeleteAsync(int id,CancellationToken ct = default);
      Task<DoorDto> UpdateAsync(Doors domain,CancellationToken ct = default);
      Task<Pagination<DoorDto>> GetDoorPaginationAsync(PaginationParams param,CancellationToken ct = default);
}