using Core.Contract.DTOs.Location;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface ILocation : IBase<LocationDto, CreateLocationDto, UpdateLocationDto>
{
  Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken ct = default);
  Task<IEnumerable<LocationDto>> GetListAsync(IEnumerable<Guid> guids, CancellationToken ct = default);

}