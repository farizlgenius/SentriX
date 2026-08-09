using Core.Application.Interfaces.Location;
using Core.Contract.DTOs.Location;
using Core.Contract.Interfaces.Location;
using SharedKernel.Domain;

namespace Core.Application.Services.Location;

public sealed class LocationService(ILocationRepository repo) : ILocation
{
  public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
  {

    var d = new Core.Domain.Entities.Location(
      dto.Name,
      dto.Description,
      dto.CountryId,
      Guid.NewGuid()
    );

        // Check name is duplicate 
    if(await repo.IsAnyByNameAsync(dto.Name))




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
    return await repo.GetCountriesAsync();
  }

  public async Task<Pagination<LocationDto>> GetPaginationAsync(PaginationParams param)
  {
    return await repo.GetPaginationAsync(param);
  }

  public async Task<LocationDto> UpdateAsync(UpdateLocationDto dto)
  {
    throw new NotImplementedException();
  }
}