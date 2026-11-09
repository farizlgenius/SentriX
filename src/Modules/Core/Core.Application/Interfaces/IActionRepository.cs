using Core.Contract.DTOs.Action;
using Core.Domain.Entities;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IActionRepository : IRepository<ActionDto,Actions>
{
      Task<IEnumerable<(int id,ActionType type,string metadata)>> GetActionDetailByGuidAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
}