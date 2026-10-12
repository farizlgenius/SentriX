using Core.Application.Interfaces;
using Core.Contract.DTOs.Procedure;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class ProcedureRepository(CoreDbContext context) : IProcedureRepository
{
      public async Task AddAsync(Procedure entity, CancellationToken ct = default)
      {
            await context.Procedures.AddAsync(
                  new Persistences.Entities.Procedure(entity)
            );

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Procedures
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Procedure.ToString(),guid.ToString());

            context.Procedures.Remove(en);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var entities = await context.Procedures
                  .Where(x => guids.Contains(x.guid))
                  .ToArrayAsync(ct);

            context.Procedures.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Procedures
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Procedure.ToString(),guid.ToString());

            en.is_active = false;
            en.updated_at = DateTime.UtcNow;

            context.Procedures.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Procedures
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Procedure.ToString(),guid.ToString());

            en.is_active = true;
            en.updated_at = DateTime.UtcNow;

            context.Procedures.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<ProcedureDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Procedures
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new ProcedureDto(
                        x.guid,
                        x.name,
                        x.procedure_actions.Select(
                              a => new ActionDto(
                                    a.action.guid,
                                    a.action.metadata
                              )
                        ).ToList(),
                        x.is_active,
                        x.is_default
                  )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Procedure.ToString(),guid.ToString());
      }

      public async Task<IEnumerable<ProcedureDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default)
      {
            return await context.Procedures
                  .AsNoTracking()
                  .Where(x => x.location.guid == locationGuid)
                  .Select(x => new ProcedureDto(
                        x.guid,
                        x.name,
                        x.procedure_actions.Select(
                              a => new ActionDto(
                                    a.action.guid,
                                    a.action.metadata
                              )
                        ).ToList(),
                        x.is_active,
                        x.is_default
                  )).ToArrayAsync();
      }

      public Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<ProcedureDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task UpdateAsync(Procedure entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}