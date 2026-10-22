using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Location;
using Core.Contract.DTOs.Output;

namespace Core.Infrastructure.Projections;

public static class LocationProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Location, LocationDto>> ToDto = 
        x => new LocationDto(
                        x.guid,
                        x.name,
                        x.description,
                        x.country_id,
                        x.country.name,
                        x.is_active,
                        x.is_default
                  );

    public static readonly Expression<Func<Persistences.Entities.Location,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}