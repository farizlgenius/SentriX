using Core.Contract.DTOs.Location;
using SharedKernel.Domain;


namespace Core.Application.Interfaces;

public interface ILocationRepository : IBaseRepository<LocationDto, Core.Domain.Entities.Location>
{
      Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken ct = default);
      Task AddDefaultOperatorAsync(Guid operatorGuid, Guid locationGuid, CancellationToken ct = default);

}