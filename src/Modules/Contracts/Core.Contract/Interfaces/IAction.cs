using Core.Contract.DTOs.Action;
using Microsoft.Extensions.Options;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IAction : IService<ActionDto,CreateActionDto,UpdateActionDto>
{
      Task<IEnumerable<OptionDto>> GetActionOptionByTypeAndDeviceAsync(Guid guid,ActionType type,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetActionOptionByDeviceAsync(Guid guid,CancellationToken ct  =default);
}