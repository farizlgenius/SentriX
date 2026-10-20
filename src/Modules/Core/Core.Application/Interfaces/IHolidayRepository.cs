using Core.Contract.DTOs.Time;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IHolidayRepository : IRepository<HolidayDto, Holiday>
{

}