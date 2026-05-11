using System;
using Location.Application.Interfaces;
using Location.Contract.DTOs;
using Location.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Location.Application.Behaviors;

public sealed class LocationBehavior(ILocationRepository repo) : ILocation
{
      public async Task<List<LocationDto>> GetAsync()
      {
            var res = await repo.GetAsync();
            return res;
      }

      public async Task<Pagination<LocationDto>> GetPaginationAsync(int Page, int PageSize, string Search)
      {
            var res = await repo.GetPaginationAsync(Page, PageSize, Search);
            return res;
      }

      public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
      {
            // Name must not be the same
            if (string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.NameEmpty);
            if (await repo.IsAnyNameAsync(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.DuplicatedName);

            // Check country id is valid
            if (!await repo.IsValidCountryAsync(dto.CountryId))
                  throw new BadRequestException(MessageHelper.Location.CountryInvalid);

            var domain = new Domain.Entities.Locations(0, StringHelper.ToCapital(dto.Name.Trim()), dto.CountryId, dto.Description);

            return await repo.AddAsync(domain);
      }

      public async Task<Pagination<CountryDto>> GetCountriesPaginationAsync(int Page, int PageSize,string Search)
      {
            var res = await repo.GetCountriesPaginationAsync(Page, PageSize, Search ?? "");
            return res;
      }

      public async Task<LocationDto> DeleteByIdAsync(int id)
      {
            if (!await repo.IsAnyByIdAsync(id))
                  throw new NotFoundException(MessageHelper.Location.LocationNotFound);

            return await repo.DeleteByIdAsync(id);
      }

      public async Task<LocationDto> UpdateAsync(UpdateLocationDto dto)
      {

            if (!await repo.IsAnyByIdAsync(dto.Id))
                  throw new NotFoundException(MessageHelper.Location.LocationNotFound);

            if (string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.NameEmpty);

            // Check country id is valid
            if (!await repo.IsValidCountryAsync(dto.CountryId))
                  throw new BadRequestException(MessageHelper.Location.CountryInvalid);

            var domain = new Domain.Entities.Locations(dto.Id, StringHelper.ToCapital(dto.Name.Trim()), dto.CountryId, dto.Description);

            return await repo.UpdateAsync(domain);


      }

      public async Task<List<LocationDto>> GetRangeLocationAsync(RangeIdDto dto)
      {
            if (dto.Ids == null || dto.Ids.Count == 0)
                  throw new BadRequestException(MessageHelper.Location.LocationInvalid);

            return await repo.GetRangeLocationAsync(dto.Ids);
      }

      public async Task<List<CountryDto>> GetAllCountriesAsync()
      {
            return await repo.GetAllCountriesAsync();
      }

      public async Task<List<LocationDto>> DeleteRangeAsync(RangeIdDto dto)
      {
            if (dto.Ids == null || dto.Ids.Count == 0)
                  throw new BadRequestException(MessageHelper.Location.LocationInvalid);

            if (!await repo.IsAllExistByIdsAsync(dto.Ids))
                  throw new NotFoundException(MessageHelper.Location.LocationNotFound);

            return await repo.DeleteRangeAsync(dto.Ids);
      }
}
