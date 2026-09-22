using Core.Contract.DTOs.Company;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IComponentMapping
{
      Task<string> GetMacByExternalIdAndEntityAndVendorAsync(int externalId,string entity,Vendor vendor,CancellationToken ct = default);
      Task<int?> GetFreeIdByMacAndEntityAndVendorAsync(string entity, Vendor vendor, int Max,IEnumerable<int>? Exceptions = default,CancellationToken ct = default);
      Task<int> GetExternalIdByMacAndEntityAsync(string mac,string entityType,CancellationToken ct= default); 
      Task InsertComponentMappingAsync(
             string entity,
            int internalId,
            int externalId,
            string mac,
            Vendor vendor,
            int locationId,
            CancellationToken ct = default
      );
}