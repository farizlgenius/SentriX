using Core.Application.Interfaces;
using Core.Contract.DTOs.Input;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using SharedKernel.Domain;

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
  InputProjections.ToDto
  ), IInputRepository
{
      public async Task<Pagination<InputDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
      param,
      x => x.location.guid == param.locationGuid,
      [x => x.name],
      ct
    );
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}