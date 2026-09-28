using Core.Application.Interfaces;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class ComponentMappingService(IComponentMappingRepository repo) : IComponentMapping
{

      public async Task<int> GetFreeIdByEntityAsync(
        string entity,
        int max,
        IEnumerable<int>? exception = default,
        CancellationToken ct = default)
      {

            return await repo.GetFreeIdByEntityAsync(entity,max,exception,ct);
      }

      public async Task<int>  GetExternalIdByMacAndEntityAsync(string mac, string entityType, CancellationToken ct = default)
      {
            var res = await repo.GetExternalIdByMacAndEntityAsync(mac,entityType);

            if(res == 0)
                  throw new NotFoundException(EntityType.ComponentMapping,$"Mac:{mac},Entity:{entityType}");

            return res;
      }

      public async Task InsertComponentMappingAsync(
            Guid guid,
            string entity,
            int externalId,
            string mac,
            Vendor vendor,
            int locationId,
            CancellationToken ct = default
      )
      {
            var d = new ComponentMappping(
                  guid,
                  entity,
                  externalId,
                  mac,
                  locationId,
                  vendor
            );

            await repo.InsertAsync(d,ct);
      }

      public async Task<string> GetMacByExternalIdAndEntityAndVendorAsync(int externalId,string entity,Vendor vendor,CancellationToken ct = default)
      {
            return await repo.GetMacByExternalIdAndEntityAndVendorAsync(externalId,entity,vendor,ct);
      }

      public async Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac,string entity, Vendor vendor, int max, IEnumerable<int>? Exceptions = null, CancellationToken ct = default)
      {
            return await repo.GetFreeIdByMacAndEntityAndVendorAsync(mac,entity,vendor,max,Exceptions,ct);
      }

      public async Task<int> GetFreeIdByEntityAndVendorAsync(string entity, Vendor vendor, int max, IEnumerable<int>? Exceptions = null, CancellationToken ct = default)
      {
            return await repo.GetFreeIdByEntityAndVendorAsync(entity,vendor,max,Exceptions,ct);
      }

      public async Task<int> GetExternalIdByGuidAndEntityAsync(Guid guid,string entity, CancellationToken ct = default)
      {
            return await repo.GetExternalIdByGuidAndEntityAsync(guid,entity,ct);
      }

      public async Task DeleteComponentMappingAsync(Guid guid, CancellationToken ct = default)
      {
            await repo.DeleteAsync(guid,ct);
      }

      public async Task UpdateExternalIdByMacAsync(string mac, short externalId,CancellationToken ct = default)
      {
            await repo.UpdateExternalIdByMacAsync(mac,externalId,ct);
      }
}