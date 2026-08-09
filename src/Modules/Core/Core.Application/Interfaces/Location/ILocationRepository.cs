using Core.Contract.DTOs.Location;

namespace Core.Application.Interfaces.Location;

public interface ILocationRepository : IBaseRepository<LocationDto, Core.Domain.Entities.Location>
{
      Task<IEnumerable<CountryDto>> GetCountriesAsync();
      Task<bool> IsAnyByNameAsync(string Name);
}