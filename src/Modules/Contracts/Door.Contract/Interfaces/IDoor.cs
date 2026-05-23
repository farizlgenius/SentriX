using Door.Contract.DTOs;
using SharedKernel.Domain;

namespace Door.Contract.Interfaces;

public interface IDoor
{
      Task<Pagination<DoorDto>> GetDoorPaginationAsync(PaginationParams param);
      Task<DoorDto> CreateAsync(CreateDoorDto dto);
      Task<DoorDto> UpdateAsync(DoorDto dto);
      Task<DoorDto> DeleteAsync(int id);
}