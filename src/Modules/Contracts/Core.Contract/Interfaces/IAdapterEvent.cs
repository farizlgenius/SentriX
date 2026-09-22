

using Core.Contract.DTOs.Events.AdapterEvent;

namespace Core.Contract.Interfaces;

public interface IAdapterEvent : IBase<AdapterEventDto, CreateAdapterEventDto, UpdateAdapterEventDto>
{

}