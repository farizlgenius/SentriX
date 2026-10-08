using Core.Contract.DTOs.Device;
using Core.Contract.DTOs.Group;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IGroupRepository : IBaseRepository<GroupDto, Core.Domain.Entities.Group>
{
      Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
      Task InsertGroupComponentAsync(Core.Domain.Entities.GroupComponent group,CancellationToken ct = default);
      Task RemoveGroupComponentAsync(Core.Domain.Entities.GroupComponent group,CancellationToken ct = default);
      Task<IEnumerable<GroupDto>> GetByMacAsync(string mac,CancellationToken ct = default);
      Task<IEnumerable<(string mac,string ip,Vendor vendor)>> GetDetailsByGroupGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default);

      
}