using Core.Contract.DTOs.Company;
using Core.Contract.DTOs.Device;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IComponentMapping
{
      Task<string> GetMacByExternalIdAndEntityAndVendorAsync(int externalId,EntityType entity,Vendor vendor,CancellationToken ct = default);
      Task<int> GetFreeIdByEntityAsync(EntityType entity, int Max,IEnumerable<int>? Exceptions = default,CancellationToken ct = default);
      Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac,EntityType entity,Vendor vendor, int Max,IEnumerable<int>? Exceptions = default,CancellationToken ct = default);
      Task<int> GetFreeIdByEntityAndVendorAsync(EntityType entity,Vendor vendor, int Max,IEnumerable<int>? Exceptions = default,CancellationToken ct = default);
      Task<int> GetExternalIdByMacAndEntityAsync(string mac,EntityType entityType,CancellationToken ct= default); 
      Task<int> GetExternalIdByGuidAndEntityAsync(Guid guid,EntityType entity, CancellationToken ct = default);
      Task InsertComponentMappingAsync(
            Guid guid,
             EntityType entity,
            int externalId,
            string mac,
            Vendor vendor,
            int locationId,
            CancellationToken ct = default
      );

      Task DeleteComponentMappingAsync(
            Guid guid,
            CancellationToken ct = default
      );

      Task UpdateExternalIdByMacAsync(
            string mac,
            short externalId,
            CancellationToken ct = default
      );

      Task<DeviceComponentDto> GetComponentByGuidAsync(Guid guid,CancellationToken ct = default);
}