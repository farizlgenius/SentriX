using Core.Contract.DTOs.Company;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IComponentMapping
{
      Task<string> GetMacByExternalIdAndEntityAndVendorAsync(int externalId,string entity,Vendor vendor,CancellationToken ct = default);
      Task<int> GetFreeIdByEntityAsync(string entity, int Max,IEnumerable<int>? Exceptions = default,CancellationToken ct = default);
      Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac,string entity,Vendor vendor, int Max,IEnumerable<int>? Exceptions = default,CancellationToken ct = default);
      Task<int> GetFreeIdByEntityAndVendorAsync(string entity,Vendor vendor, int Max,IEnumerable<int>? Exceptions = default,CancellationToken ct = default);
      Task<int> GetExternalIdByMacAndEntityAsync(string mac,string entityType,CancellationToken ct= default); 
      Task<int> GetExternalIdByGuidAndEntityAsync(Guid guid,string entity, CancellationToken ct = default);
      Task InsertComponentMappingAsync(
            Guid guid,
             string entity,
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
}