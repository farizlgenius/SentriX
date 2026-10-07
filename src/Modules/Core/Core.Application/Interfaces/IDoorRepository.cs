using Core.Contract.DTOs.Door;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IDoorRepository : IBaseRepository<DoorDto, Door>
{
      Task<bool> CheckRelationAsync(Guid guid, CancellationToken ct = default);
      Task<Dictionary<Guid, int>> GetDoorIdsMapGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
      Task<IEnumerable<(Guid guid,int id,string mac,string ip,Vendor vendor)>> GetDetailsByGuidAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
      Task<IEnumerable<DoorDto>> GetByMacAsync(string mac,CancellationToken ct = default);

}