using Core.Application.Interfaces;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Services;

public class BaseService<TDto, TDomain>(IRepository<TDto, TDomain> repo) : IBaseService<TDto, TDomain> where TDto : class where TDomain : class
{
      public async Task<TDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetAsync(guid,ct);
      }

      public async Task<IEnumerable<TDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetByLocationAsync(guid,ct);
      }

      public async Task<Pagination<TDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetPaginationAsync(param,ct);
      }
}