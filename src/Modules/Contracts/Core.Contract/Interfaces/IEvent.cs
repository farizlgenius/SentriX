using Core.Contract.DTOs.Event;

namespace Core.Contract.Interfaces;

public interface IEvent : IBase<EventDto, CreateEventDto, UpdateEventDto>
{

}