using Core.Contract.DTOs.Door;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IDoorRepository : IBaseRepository<DoorDto, Door>
{
      Task<bool> CheckRelationAsync(Guid guid, CancellationToken ct = default);
      Task<Dictionary<Guid, int>> GetDoorIdsMapGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
}