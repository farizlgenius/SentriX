using Core.Contract.DTOs.Action;
using Core.Contract.DTOs.Trigger;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface ITrigger : IBase<TriggerDto,CreateTriggerDto,UpdateTriggerDto>
{
      Task<IEnumerable<OptionDto>> GetSourceAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetTypeAsync(Guid source,CancellationToken ct =default);
      Task<IEnumerable<OptionDto>> GetCodeAsync(Guid type,CancellationToken ct = default);
}