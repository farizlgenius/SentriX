using Core.Contract.DTOs.Action;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IActionRepository : IRepository<ActionDto,Actions>
{
      Task<IEnumerable<(int id, ActionType type,short? externalId,string? metadata,int sequence)>> GetActionDetailByGuidAsync(IEnumerable<(Guid guid,int sequence)> actions, CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetActionOptionByTypeAndDeviceAsync(Guid guid,ActionType type,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetActionOptionByDeviceAsync(Guid guid,CancellationToken ct = default);
}