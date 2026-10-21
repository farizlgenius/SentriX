using Core.Contract.DTOs.Input;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IInputRepository : IRepository<InputDto, Input>
{

}