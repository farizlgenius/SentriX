using Core.Application.Interfaces;
using Core.Contract.DTOs.Input;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
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

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default)
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
}