using Core.Contract.DTOs.Location;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces.Location;

public interface ILocation : IBase<LocationDto, CreateLocationDto, UpdateLocationDto>
{
  Task<IEnumerable<CountryDto>> GetCountriesAsync();

}