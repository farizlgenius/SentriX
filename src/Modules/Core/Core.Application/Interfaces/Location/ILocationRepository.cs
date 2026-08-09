using Core.Contract.DTOs.Location;
using SharedKernel.Domain;


namespace Core.Application.Interfaces.Location;

public interface ILocationRepository : IBaseRepository<LocationDto, Core.Domain.Entities.Location>
{
      Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken ct = default);
      Task<Pagination<LocationDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);

}