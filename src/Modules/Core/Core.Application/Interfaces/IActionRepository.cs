using Core.Contract.DTOs.Action;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IActionRepository : IRepository<ActionDto, Domain.Entities.Action>
{
      Task<IEnumerable<OptionDto>> GetActionOptionByTypeAndDeviceAsync(Guid guid,ActionType type,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetActionOptionByDeviceAsync(Guid guid,CancellationToken ct = default);
}