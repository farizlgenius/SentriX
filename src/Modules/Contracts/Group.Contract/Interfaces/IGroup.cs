using Group.Contract.DTOs;
using SharedKernel.Domain;

namespace Group.Contract.Interfaces;

public interface IGroup
{
      Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param);
      Task<GroupDto> CreateAsync(CreateGroupDto dto);
      Task<GroupDto> DeleteAsync(int id);
      Task<GroupDto> UpdateAsync(GroupDto dto);
}