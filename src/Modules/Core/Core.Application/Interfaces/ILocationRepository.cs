using Core.Contract.DTOs.Location;
using SharedKernel.Domain;


namespace Core.Application.Interfaces;

public interface ILocationRepository : IRepository<LocationDto, Core.Domain.Entities.Location>
{
      Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken ct = default);
      Task<IEnumerable<LocationDto>> GetListAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
      Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
      Task<IEnumerable<Guid>> IsAnyInvalidGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);

}