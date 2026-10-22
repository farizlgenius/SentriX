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
  public async Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await base.GetPaginationAsync(
      param,
      x => x.location == null || x.location.guid == param.locationGuid,
      [x => x.name],
      ct
    );
  }

  public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

}