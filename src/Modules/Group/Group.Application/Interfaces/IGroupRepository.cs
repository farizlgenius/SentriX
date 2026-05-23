using Group.Contract.DTOs;
using Group.Domain.Entities;

namespace Group.Application.Interfaces;

public interface IGroupRepository
{
      Task<short> GetLowestGroupComponentIdAsync(CancellationToken ct = default);
      Task<GroupDto> CreateAsync(Groups dto,CancellationToken ct = default);
      Task<GroupDto> UpdateAsync(Groups dto,CancellationToken ct = default);
      Task<GroupDto> DeleteAsync(int id,CancellationToken ct = default);
      Task<bool> IsAnyByIdAsync(int id,CancellationToken ct = default);
      Task<GroupDto> GetByIdAsync(int id,CancellationToken ct = default);
}