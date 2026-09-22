using Core.Application.Interfaces;
using Core.Contract.DTOs.Events.AdapterEvent;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class AdapterEventService(IAdapterEventRepository repo) : IAdapterEvent
{
  public async Task<Guid> CreateAsync(CreateAdapterEventDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<AdapterEventDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<AdapterEventDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<AdapterEventDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateAdapterEventDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}