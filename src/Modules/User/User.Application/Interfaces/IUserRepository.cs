using SharedKernel.Domain;
using User.Contract.DTOs;
using User.Domain.Entities;

namespace User.Application.Interfaces;

public interface IUserRepository
{
      Task AddAsync(Users domain,CancellationToken ct = default);
      Task<UserDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyUserByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<bool> IsAnyUserByIdentificationAsync(string identification,CancellationToken ct = default);

      Task UpdateImagePathAsync(string path,string userid,CancellationToken ct = default);

      Task DeleteAsync(Guid guid,CancellationToken ct = default);
      Task<Pagination<UserDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task UpdateAsync(Domain.Entities.Users user,CancellationToken ct = default);
       Task<IEnumerable<OptionDto>> GetUserFlagOptionAsync(CancellationToken ct = default);  
       Task<bool> IsAnyUserNotSyncAsync(IEnumerable<Guid> GpIds, int LocationId, DateTime SyncAt, CancellationToken ct = default);
       Task<IEnumerable<UserDto>> GetUserByGroupGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
}