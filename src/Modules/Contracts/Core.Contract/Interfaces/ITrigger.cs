using Core.Contract.DTOs.Action;
using Core.Contract.DTOs.Trigger;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface ITrigger : IBase<TriggerDto,CreateTriggerDto,UpdateTriggerDto>
{
      Task<IEnumerable<OptionDto>> GetSourceAsync();
      Task<IEnumerable<OptionDto>> GetTypeAsync(short source);
      Task<IEnumerable<OptionDto>> GetCodeAsync(short type);
}