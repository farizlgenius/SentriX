using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Trigger;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class TriggerRepository(CoreDbContext context) : ITriggerRepository
{
      public Task AddAsync(Trigger entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<TriggerDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<TriggerDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<OptionDto>> GetCodeAsync(Guid type, CancellationToken ct = default)
      {
            return await context.TriggerCodes
                  .AsNoTracking()
                  .Where(x => x.trigger_type == null || x.trigger_type.guid == type)
                  .Select(x => new OptionDto(
                        x.name,
                        x.value,
                        x.guid.ToString()
                  )).ToArrayAsync();
      }

      public Task<CommandDetailDto> GetCommandDetailAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<short> GetExternalIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Dictionary<Guid, short>> GetExternalIdMapGuidByGuidRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<short> GetFreeExeternalIdAsync(int max, IEnumerable<short>? exception = null, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<(int, short)> GetIdAndExternalIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<TriggerDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<short>> GetRangeExernalIdByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<OptionDto>> GetSourceAsync(CancellationToken ct = default)
      {
            return await context.TriggerSources
                  .AsNoTracking()
                  .Select(x => new OptionDto(
                        x.name,
                        x.value,
                        x.guid.ToString()
                  )).ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetTypeAsync(Guid source, CancellationToken ct = default)
      {
            return await context.TriggerTypes
                  .AsNoTracking()
                  .Where(x => x.trigger_source == null || x.trigger_source.guid == source)
                  .Select(x => new OptionDto(
                        x.name,
                        x.value,
                        x.guid.ToString()
                  )).ToArrayAsync();
      }

      public Task<bool> IsAnyByNameAndLocationGuidAsync(string name, Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByNameAsync(string name, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsDisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsEnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task UpdateAsync(Trigger entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}