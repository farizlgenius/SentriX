namespace Core.Application.Interfaces;

public interface IGroupRepository 
// : IBaseRepository<GroupDto, Core.Domain.Entities.Group>
{
      Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
}