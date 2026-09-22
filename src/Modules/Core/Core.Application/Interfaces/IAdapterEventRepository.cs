
using Core.Contract.DTOs.Events.AdapterEvent;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IAdapterEventRepository : IBaseRepository<AdapterEventDto, AdapterEvent>
{
  // Task UpdateStatusByMacAndComponentIdAndTagAndCommandAsync(
  //   string mac,
  //   int componentId,
  //   int tag,
  //   string command
  // );
}