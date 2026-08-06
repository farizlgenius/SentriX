using Core.Application.Interfaces.Location;
using Core.Contract.DTOs.Location;
using Core.Contract.Interfaces.Location;
using SharedKernel.Domain;

namespace Core.Application.Services.Location;

public sealed class LocationService(ILocationRepository repo) : ILocation
{
  public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
  {
    throw new NotImplementedException();
  }

  public async Task<LocationDto> DeleteByGuidAsync(Guid guid)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<LocationDto>> DeleteRangeAsync(IEnumerable<Guid> guids)
  {
    throw new NotImplementedException();
  }

  public async Task<LocationDto> GetByGuidAsync(Guid guid)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<LocationDto>> GetPaginationAsync(PaginationParams param)
  {
    throw new NotImplementedException();
  }

  public async Task<LocationDto> UpdateAsync(UpdateLocationDto dto)
  {
    throw new NotImplementedException();
  }
}