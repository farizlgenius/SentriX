using Core.Contract.DTOs.Action;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IActionRepository : IRepository<ActionDto,Actions>
{
      
}