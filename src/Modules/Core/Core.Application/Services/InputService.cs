using Core.Application.Interfaces;
using Core.Contract.DTOs.Input;
using Core.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class InputService(
      IInputRepository repo
) : BaseService<InputDto, Domain.Entities.Input>(
      repo
), IInput
{
      public Task<Guid> CreateAsync(CreateInputDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Guid> UpdateAsync(UpdateInputDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}