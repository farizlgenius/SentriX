using System;
using Location.Contract.DTOs;
using Location.Domain.Entities;
using SharedKernel.Domain;

namespace Location.Application.Interfaces;

public interface ILocationRepository
{
      Task<List<LocationDto>> GetAsync();
      Task<Pagination<LocationDto>> GetPaginationAsync(int Page, int PageSize, string Search);
      Task<Pagination<CountryDto>> GetCountriesPaginationAsync(int Page, int PageSize, string Search);
      Task<bool> IsAnyNameAsync(string name);
      Task<LocationDto> AddAsync(Locations location);
      Task<bool> IsValidCountryAsync(int id);
      Task<bool> IsAnyByIdAsync(int id);
      Task<LocationDto> DeleteByIdAsync(int id);
      Task<LocationDto> UpdateAsync(Locations location);
      Task<List<LocationDto>> GetRangeLocationAsync(List<int> ids);
      Task<List<CountryDto>> GetAllCountriesAsync();
      Task<bool> IsAllExistByIdsAsync(List<int> ids);
      Task<List<LocationDto>> DeleteRangeAsync(List<int> ids);
}
