using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IComponentMappingRepository
{
  Task AddAsync(ComponentMappping entity, CancellationToken ct = default);
  Task GetFreeIdByMacAsync(string mac,CancellationToken ct = default);
  Task GetExternalIdByMacAsync(string mac,CancellationToken ct = default);
}