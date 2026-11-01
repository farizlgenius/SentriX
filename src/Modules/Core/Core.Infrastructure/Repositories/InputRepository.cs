using Core.Application.Interfaces;
using Core.Contract.DTOs.Input;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class InputRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Input, Domain.Entities.Input, InputDto>(
  context,
  d => new Persistences.Entities.Input(d),
  (en, dom) =>
  {
        en.name = dom.Name;
        en.slot_no = dom.SlotNo;
        en.vendor = dom.Vendor;
        en.metadata = dom.Metadata;
        en.location_id = dom.LocationId;
        en.device_module_id = dom.DeviceModuleId;
        en.updated_at = DateTime.UtcNow;
  },
  InputProjections.ToDto,
  InputProjections.ToCommand
  ), IInputRepository
{
      public async Task<Pagination<InputDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
      param,
      param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
      [x => x.name],
      ct
    );
      }

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
           return false;
      }

      public async Task<short> GetFreeExeternalIdByMacAsync(string mac, int max, IEnumerable<short>? exception = null, CancellationToken ct = default)
      {
            var existingIds = await Context.Inputs
                  .AsNoTracking()
                  .Where(x => x.device_module.device.mac == mac)
                  .Select(x => x.external_id!.Value)
                  .ToArrayAsync(ct);

            var existIdsHash = existingIds.ToHashSet();

            if (exception != null)
            {
                  existIdsHash.UnionWith(exception);
            }

            for (short id = 1; id < max; id++)
            {
                  if (!existIdsHash.Contains(id))
                        return id;
            }

            throw new ExceedException(typeof(Persistences.Entities.Input).Name);
      }

      public async Task<string> GetNameByExternalIdAndMacAndVendorAsync(short externalId, string mac, Vendor vendor, CancellationToken ct = default)
      {
            var res = await Context.Inputs
                  .AsNoTracking()
                  .Where(x => x.external_id == externalId && x.device_module.device.mac == mac && x.vendor == vendor)
                  .Select(x => x.name)
                  .FirstOrDefaultAsync(ct) ?? "Unknown";

            return res;
      }

      public async Task<Guid> GuidByExternalIdAndDeviceExternalIdAndVendorAsync(short externalId, short deviceExternalId, Vendor vendor, CancellationToken ct = default)
      {
            var res = await Context.Inputs
                  .AsNoTracking()
                  .Where(x => x.external_id == externalId && x.device_module.device.external_id == deviceExternalId && x.vendor == vendor)
                  .Select(x => x.guid)
                  .FirstOrDefaultAsync(ct);

            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Inputs
                  .AsNoTracking()
                  .Where(x => x.device_module.device.guid == guid)
                  .Select(x => new OptionDto(
                        x.name,
                        x.guid,
                        x.device_module.device.mac
                  ))
                  .ToArrayAsync(ct);
      }
}