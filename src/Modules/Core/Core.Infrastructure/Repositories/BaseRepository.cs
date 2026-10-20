using System.Linq.Expressions;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity,TDomain,TDto>(
      CoreDbContext context,
      Func<TDomain, TEntity> toEntity,
      Action<TEntity, TDomain> updateEntity,
      Expression<Func<TEntity,TDto>> toDto
      ) : IBaseRepository<TEntity,TDomain,TDto> where TEntity : BaseEntity where TDto : class where TDomain : BaseDomain
{
      protected readonly CoreDbContext Context = context;
      protected readonly Expression<Func<TEntity, TDto>> ToDto = toDto;
      protected readonly Func<TDomain, TEntity> ToEntity = toEntity;
      protected readonly Action<TEntity, TDomain> UpdateEntityDelegate = updateEntity;

      public async Task AddAsync(TDomain domain, CancellationToken ct = default)
      {
            await Context.Set<TEntity>()
                  .AddAsync(
                        ToEntity(domain),
                        ct);

            await Context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await Context.Set<TEntity>()
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) 
                  ?? throw new NotFoundException(typeof(TEntity).Name, guid.ToString());

            Context.Set<TEntity>().Remove(en);

            await Context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var ens = await Context.Set<TEntity>()
                  .Where(x => guids.Contains(x.guid))
                  .ToArrayAsync(ct);

            Context.Set<TEntity>().RemoveRange(ens);

            await Context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await Context.Set<TEntity>()
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) 
                  ?? throw new NotFoundException(typeof(TEntity).Name, guid.ToString());

            en.is_active = false;
            en.updated_at = DateTime.UtcNow;
            return await Context.SaveChangesAsync(ct) > 0;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await Context.Set<TEntity>()
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) 
                  ?? throw new NotFoundException(typeof(TEntity).Name, guid.ToString());

            en.is_active = true;
            en.updated_at = DateTime.UtcNow;
            return await Context.SaveChangesAsync(ct) > 0;
      }

      public async Task<TDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .Where(x => x.guid == guid)
                  .Select(ToDto)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(typeof(TEntity).Name, guid.ToString());
      }

      public async Task<IEnumerable<TDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .Where(x => x.location == null || x.location.guid == locationGuid)
                  .Select(ToDto)
                  .ToArrayAsync(ct);
      }

      public async Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            var res = await Context.Set<TEntity>()
                  .Where(x => x.id == id)
                  .Select(x => x.guid)
                  .FirstOrDefaultAsync(ct);

            if(res == Guid.Empty)
                  throw new NotFoundException(typeof(TEntity).Name,id.ToString());

            return res;
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.Set<TEntity>()
                  .Where(x => x.guid == guid)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync(ct);

            if(res == 0)
                  throw new NotFoundException(typeof(TEntity).Name,guid.ToString());

            return res;
      }

      public Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyByNameAndLocationGuidAsync(string name, Guid locationGuid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AnyAsync(x =>  (x.location == null || x.location.guid == locationGuid) && x.name == name,ct);
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AnyAsync(x => x.guid == guid,ct);
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AnyAsync(x => x.guid == guid && x.is_default,ct);
      }

      public async Task<bool> IsDisabledAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AnyAsync(x => x.guid == guid && x.is_active == false,ct);
      }

      public async Task<bool> IsEnabledAsync(Guid guid, CancellationToken ct = default)
      {
           return await Context.Set<TEntity>()
                  .AnyAsync(x => x.guid == guid && x.is_active == true,ct);
      }

      public async Task UpdateAsync(TDomain domain, CancellationToken ct = default)
      {
            var en = await Context.Set<TEntity>()
                  .Where(x => x.guid == domain.Guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(typeof(TEntity).Name,domain.Guid.ToString());

            UpdateEntityDelegate(en,domain);

            Context.Set<TEntity>().Update(en);
            await Context.SaveChangesAsync(ct);
      }
}