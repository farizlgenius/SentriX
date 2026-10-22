using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Company;
using Core.Contract.DTOs.Department;

namespace Core.Infrastructure.Projections;

public static class DepartmentProjections
{
      public static readonly Expression<Func<Persistences.Entities.Department,DepartmentDto>> ToDto = 
      x => new DepartmentDto(
            x.guid,
            x.name,
            x.description,
            x.company.guid,
            x.company.name,
            x.is_active,
            x.is_default
      );

      public static readonly Expression<Func<Persistences.Entities.Department,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}