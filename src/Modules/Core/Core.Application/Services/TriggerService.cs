using Core.Application.Interfaces;
using Core.Contract.DTOs.Trigger;
using Core.Contract.Interfaces;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class TriggerService(ITriggerRepository repo) : ITrigger
{
      public async Task<Guid> CreateAsync(CreateTriggerDto dto, CancellationToken ct = default)
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

      public async Task<TriggerDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<TriggerDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<OptionDto>> GetCodeAsync(Guid type,CancellationToken ct = default)
      {
            return await repo.GetCodeAsync(type,ct);
      }

      public async Task<Pagination<TriggerDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<OptionDto>> GetSourceAsync(CancellationToken ct = default)
      {
            return await repo.GetSourceAsync(ct);
      }

      public async Task<IEnumerable<OptionDto>> GetTypeAsync(Guid source,CancellationToken ct = default)
      {
            return await repo.GetTypeAsync(source,ct);
      }

      public async Task<Guid> UpdateAsync(UpdateTriggerDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}