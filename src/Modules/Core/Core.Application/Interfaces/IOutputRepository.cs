using Core.Contract.DTOs.Output;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IOutputRepository : IBaseRepository<OutputDto, Output>
{

}