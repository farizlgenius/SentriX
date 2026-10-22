using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Company;

namespace Core.Infrastructure.Projections;

public static class CompanyProjections
{
      public static readonly Expression<Func<Persistences.Entities.Company,CompanyDto>> ToDto = 
      x => new CompanyDto(
            x.guid,
            x.name,
            x.address,
            x.description,
            x.is_active,
            x.is_default
      );

      public static readonly Expression<Func<Persistences.Entities.Company,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}