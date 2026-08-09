using Core.Application.Interfaces.Location;
using Core.Contract.DTOs.Location;
using Core.Contract.Interfaces.Location;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services.Location;

public sealed class LocationService(ILocationRepository repo) : ILocation
{
  public async Task<LocationDto> CreateAsync(CreateLocationDto dto, CancellationToken ct = default)
  {
    var d = new Core.Domain.Entities.Location(
      dto.Name,
      dto.Description,
      dto.CountryId,
      Guid.NewGuid()
    );

    // Check name is duplicate 
    if (await repo.IsAnyByNameAsync(dto.Name))
      throw new DuplicateException(EntityType.Location, dto.Name);

    await repo.AddAsync(d, ct);

    return new LocationDto(
      d.Guid,
      d.Name,
      d.Description,
      d.CountryId,
      d.IsActive,
      d.IsDefault
    );
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Location, guid.ToString());

    // Check relate object here

    await repo.DeleteAsync(guid, ct);

    return true;
  }

  public async Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new BadRequestException("Guids is empty.");

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<LocationDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken ct = default)
  {
    return await repo.GetCountriesAsync(ct);
  }

  public async Task<Pagination<LocationDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<LocationDto> UpdateAsync(UpdateLocationDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.Location, dto.Guid.ToString());

    var d = new Core.Domain.Entities.Location(
      dto.Name,
      dto.Description,
      dto.CountryId,
      dto.Guid
    );

    await repo.UpdateAsync(d);

    return new LocationDto(
      dto.Guid,
      dto.Name,
      dto.Description,
      dto.CountryId,
      dto.IsActive,
      dto.IsDefault
    );


  }
}