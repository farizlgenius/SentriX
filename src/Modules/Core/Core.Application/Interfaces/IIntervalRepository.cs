using Core.Contract.DTOs.Time;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IIntervalRepository : IBaseRepository<IntervalDto, Interval>
{
  Task<bool> IsAnySameDataSetAsync(TimeOnly start, TimeOnly end, DayInWeek day, CancellationToken ct = default);
  Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
}