using Core.Application.Interfaces;
using Core.Contract.DTOs.Position;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class PositionRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Position, Domain.Entities.Position, PositionDto>(
      context,
      d => new Persistences.Entities.Position(d),
      (en, dom) =>
      {
            en.name = dom.Name;
            en.description = dom.Description;
            en.updated_at = DateTime.UtcNow;
      },
      PositionProjections.ToDto,
      PositionProjections.ToCommand
), IPositionRepository
{


      public async Task<Pagination<PositionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Pagination<PositionDto>> GetPaginationByDepartmentGuidAsync(PaginationParams param, Guid guid, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(param, x => x.department.guid == guid, [x => x.name, x => x.description]
, ct);
      }



      public async Task<bool> IsAnyNameByDepartmentGuidAsync(string name, Guid guid, CancellationToken ct = default)
      {
            return await Context.Positions
        .AsNoTracking()
        .AnyAsync(x => x.name.Equals(name) && x.department.guid == guid, cancellationToken: ct);
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyUserAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Positions
              .AsNoTracking()
              .AnyAsync(x => x.guid == guid && x.users.Any(), cancellationToken: ct);
      }


}