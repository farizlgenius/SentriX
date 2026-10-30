using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Output;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class OutputRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Output, Domain.Entities.Output, OutputDto>(
  context,
  d => new Persistences.Entities.Output(d),
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
  OutputProjections.ToDto,
  OutputProjections.ToCommand
  ), IOutputRepository
{
      public async Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default)
      {
            var existingIds = await Context.Outputs
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

            throw new ExceedException(typeof(Persistences.Entities.Output).Name);
      }

      public async Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
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
    throw new NotImplementedException();
  }

      public async Task<Guid> OutputGuidByExternalIdAndDeviceExternalIdAndVendorAsync(short externalId, short deviceExternalId, Vendor vendor, CancellationToken ct = default)
      {
            return await Context.Outputs
              .AsNoTracking()
              .Where(x => x.external_id == externalId && x.device_module.device.external_id == deviceExternalId && x.vendor == vendor)
              .Select(x => x.guid)
              .FirstOrDefaultAsync(ct);
      }
}