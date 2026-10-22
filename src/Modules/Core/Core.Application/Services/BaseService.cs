using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Services;

public class BaseService<TDto, TDomain>(
      IRepository<TDto, TDomain> repo
      ) : IBaseService<TDto,TDomain> where TDto : class where TDomain : BaseDomain 
{
      protected readonly IRepository<TDto,TDomain> Repo = repo;


      public async Task<TDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await Repo.GetAsync(guid,ct);
      }

      public async Task<IEnumerable<TDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            return await Repo.GetByLocationAsync(guid,ct);
      }

      public async Task<Pagination<TDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await Repo.GetPaginationAsync(param,ct);
      }


}