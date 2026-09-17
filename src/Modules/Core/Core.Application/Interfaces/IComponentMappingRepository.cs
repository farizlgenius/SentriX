using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IComponentMappingRepository
{
  Task AddAsync(ComponentMappping entity, CancellationToken ct = default);
  Task GetFreeIdByMacAsync(string mac, CancellationToken ct = default);
  Task GetExternalIdByMacAsync(string mac, CancellationToken ct = default);
  Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac, string entity, Vendor vendor, int max, CancellationToken ct = default);
  Task<IEnumerable<int>> GetExternalIdsByEntityAndVendorAsync(string entity, Vendor vendor, CancellationToken ct = default);
  Task<int> GetExternalIdByMacAndEntityAsync(string mac,string entity,CancellationToken ct = default);
}