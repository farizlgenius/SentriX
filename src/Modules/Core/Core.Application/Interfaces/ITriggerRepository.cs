using Core.Contract.DTOs.Trigger;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface ITriggerRepository : IBaseRepository<TriggerDto,Trigger>
{
      Task<IEnumerable<OptionDto>> GetSourceAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetTypeAsync(Guid source,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetCodeAsync(Guid type,CancellationToken ct = default);
}