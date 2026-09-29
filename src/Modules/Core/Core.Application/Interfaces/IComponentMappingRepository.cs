using Core.Contract.DTOs.Device;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IComponentMappingRepository
{
  Task InsertAsync(ComponentMappping entity, CancellationToken ct = default);
  Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac, EntityType entity, Vendor vendor, int max,IEnumerable<int>? exception = default, CancellationToken ct = default);
  Task<int> GetFreeIdByEntityAndVendorAsync(EntityType entity, Vendor vendor, int max,IEnumerable<int>? exception = default, CancellationToken ct = default);
  Task<int> GetFreeIdByEntityAsync(EntityType entity,int max,IEnumerable<int>? exception = default, CancellationToken ct = default);
  Task<IEnumerable<int?>> GetExternalIdsByEntityAndVendorAsync(EntityType entity, Vendor vendor, CancellationToken ct = default);
  Task<int> GetExternalIdByMacAndEntityAsync(string mac,EntityType entity,CancellationToken ct = default);
  Task<int> GetExternalIdByGuidAndEntityAsync(Guid guid,EntityType entity,CancellationToken ct = default);
  Task<string> GetMacByExternalIdAndEntityAndVendorAsync(
     int externalId,
        EntityType entity,
        Vendor vendor, CancellationToken ct = default);

   Task DeleteAsync(Guid guid,CancellationToken ct = default);
   Task<Guid> GetGuidByExternalIdAndEntityAndVendorAsync(short externalId,EntityType entity,Vendor vendor,CancellationToken ct = default);
   Task UpdateExternalIdByMacAsync(string mac,short externalId,CancellationToken ct = default);
   Task<DeviceComponentDto> GetComponentByGuidAsync(Guid guid,CancellationToken ct = default);
}