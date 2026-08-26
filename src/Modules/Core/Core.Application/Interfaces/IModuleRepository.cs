namespace Core.Application.Interfaces;

public interface IModuleRepository
{
  Task<Dictionary<Guid, (int, string)>> GetMapGuidAndMapByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
}