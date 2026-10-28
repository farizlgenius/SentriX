using Core.Contract.DTOs.Area;

namespace Core.Application.Interfaces;

public interface IAreaRepository : IRepository<AreaDto, Domain.Entities.Area>
{
      Task<Dictionary<Guid,int>> GetAreaIdMapGuidByGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
}