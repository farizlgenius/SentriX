using Core.Contract.DTOs.Time;

namespace Core.Application.Interfaces;

public interface ITimeRepository : IBaseRepository<TimeZoneDto, Domain.Entities.TimeZone>
{

}