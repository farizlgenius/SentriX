using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IComponentMappingRepository
{
  Task InsertAsync(ComponentMappping entity, CancellationToken ct = default);
  Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac, string entity, Vendor vendor, int max,IEnumerable<int>? exception = default, CancellationToken ct = default);
  Task<int> GetFreeIdByEntityAndVendorAsync(string entity, Vendor vendor, int max,IEnumerable<int>? exception = default, CancellationToken ct = default);
  Task<int> GetFreeIdByEntityAsync(string entity,int max,IEnumerable<int>? exception = default, CancellationToken ct = default);
  Task<IEnumerable<int?>> GetExternalIdsByEntityAndVendorAsync(string entity, Vendor vendor, CancellationToken ct = default);
  Task<int> GetExternalIdByMacAndEntityAsync(string mac,string entity,CancellationToken ct = default);
  Task<int> GetExternalIdByGuidAndEntityAsync(Guid guid,string entity,CancellationToken ct = default);
  Task<string> GetMacByExternalIdAndEntityAndVendorAsync(
     int externalId,
        string entity,
        Vendor vendor, CancellationToken ct = default);

   Task DeleteAsync(Guid guid,CancellationToken ct = default);
   Task<Guid> GetGuidByExternalIdAndEntityAndVendorAsync(short externalId,string entity,Vendor vendor,CancellationToken ct = default);
   Task UpdateExternalIdByMacAsync(string mac,short externalId,CancellationToken ct = default);
}