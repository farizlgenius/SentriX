using System;
using Location.Contract.DTOs;
using SharedKernel.Domain;

namespace Location.Contract.Interfaces;

public interface ILocation
{
      Task<List<LocationDto>> GetAsync();
      Task<Pagination<LocationDto>> GetPaginationAsync(int Page, int PageSize, string Search);
      Task<LocationDto> CreateAsync(CreateLocationDto dto);
      Task<Pagination<CountryDto>> GetCountriesPaginationAsync(int Page, int PageSize,string Serach);
      Task<LocationDto> DeleteByIdAsync(int id);
      Task<LocationDto> UpdateAsync(UpdateLocationDto dto);
      Task<List<LocationDto>> GetRangeLocationAsync(RangeIdDto dto);
      Task<List<CountryDto>> GetAllCountriesAsync();
      Task<List<LocationDto>> DeleteRangeAsync(RangeIdDto dto);
}
