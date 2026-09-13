using Core.Contract.DTOs.Event;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IEventRepository : IBaseRepository<EventDto, Event>
{

}