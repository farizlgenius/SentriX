using Core.Contract.DTOs.Door;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IDoorRepository : IBaseRepository<DoorDto, Door>
{

}