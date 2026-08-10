using Core.Contract.DTOs.Department;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IDepartmentRepository : IBaseRepository<DepartmentDto, Department>
{

}