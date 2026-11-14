using Core.Application.Interfaces;
using Core.Contract.DTOs.Trigger;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class TriggerVariableService(
      ITriggerVariableRepository repo
) : BaseService<TriggerVariableDto,TriggerVariable>(repo),ITriggerVariable
{
      public async Task<Guid> CreateAsync(CreateTriggerVariableDto dto, CancellationToken ct = default)
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


      public async Task<Guid> UpdateAsync(UpdateTriggerVariableDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}