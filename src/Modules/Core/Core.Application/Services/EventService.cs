using Core.Application.Interfaces;
using Core.Contract.DTOs.Event;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class EventService(IEventRepository repo) : IEvent
{
  public async Task<Guid> CreateAsync(CreateEventDto dto, CancellationToken ct = default)
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

  public async Task<EventDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<EventDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<EventDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateEventDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}