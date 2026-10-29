using System.Linq.Expressions;
using Core.Application.Interfaces;
using Core.Contract.DTOs;
using Core.Domain.Entities;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity, TDomain, TDto>(
      CoreDbContext context,
      Func<TDomain, TEntity> toEntity,
      Action<TEntity, TDomain> updateEntity,
      Expression<Func<TEntity, TDto>> toDto,
      Expression<Func<TEntity,CommandDetailDto>> toCommand
      ) : IBaseRepository<TEntity, TDomain, TDto> where TEntity : BaseEntity where TDto : class where TDomain : BaseDomain
{
      protected readonly CoreDbContext Context = context;
      // protected readonly Expression<Func<TEntity, TDto>> ToDto = toDto;
      // protected readonly Func<TDomain, TEntity> ToEntity = toEntity;
      // protected readonly Action<TEntity, TDomain> UpdateEntityDelegate = updateEntity;
      // protected readonly Expression<Func<TEntity, bool>>? Filter = filter;

      public async Task<(int,short)> GetIdAndExternalIdByGuidAsync(Guid guid,CancellationToken ct = default)
      {
            var res = await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(typeof(TEntity).Name, guid.ToString());

            return (res.id,res.external_id ?? -1);
      }


      public virtual async Task AddAsync(TDomain domain, CancellationToken ct = default)
      {
            await Context.Set<TEntity>()
                  .AddAsync(
                        toEntity(domain),
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

      public async Task<Dictionary<Guid,short>> GetExternalIdMapGuidByGuidRangeAsync(IEnumerable<Guid> guids,CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .ToDictionaryAsync(x => x.guid, x => x.external_id!.Value, cancellationToken: ct);
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
                  .Select(toDto)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(typeof(TEntity).Name, guid.ToString());
      }

      public async Task<IEnumerable<TDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .Where(x => x.location == null || x.location.guid == locationGuid)
                  .Select(toDto)
                  .ToArrayAsync(ct);
      }

      public async Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            var res = await Context.Set<TEntity>()
                  .Where(x => x.id == id)
                  .Select(x => x.guid)
                  .FirstOrDefaultAsync(ct);

            if (res == Guid.Empty)
                  throw new NotFoundException(typeof(TEntity).Name, id.ToString());

            return res;
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.Set<TEntity>()
                  .Where(x => x.guid == guid)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync(ct);

            if (res == 0)
                  throw new NotFoundException(typeof(TEntity).Name, guid.ToString());

            return res;
      }

      public Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Pagination<TDto>> GetPaginationAsync(
            PaginationParams param,
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, string>>[]? searchFields = null,
            CancellationToken ct = default
      )
      {
            var query = Context.Set<TEntity>().AsNoTracking();

            // 1. Primary Filter
            if (filter is not null)
            {
                  query = query.Where(filter);
            }

            // 2. Generic Search
            if (!string.IsNullOrWhiteSpace(param.search) && searchFields is { Length: > 0 })
            {
                  var search = param.search.Trim();
                  var isNpgsql = Context.Database.IsNpgsql();
                  var searchExpression = BuildSearchExpression(searchFields, search, isNpgsql);
                  query = query.Where(searchExpression);
            }

            // 3. Date Range Filtering
            if (param.startDate.HasValue)
            {
                  var startUtc = DateTime.SpecifyKind(param.startDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.created_at >= startUtc);
            }

            if (param.endDate.HasValue)
            {
                  var endUtc = DateTime.SpecifyKind(param.endDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.created_at <= endUtc);
            }

            // 4. Automated Dynamic String Sorting
            query = query.OrderByProperty(param.sortBy, param.isDescending);

            // 5. Count & Paginate
            var count = await query.CountAsync(ct);

            var items = await query
                .Skip((param.pageNumber - 1) * param.pageSize)
                .Take(param.pageSize)
                .Select(toDto)
                .ToListAsync(ct);

            var totalPages = (int)Math.Ceiling(count / (double)param.pageSize);

            return new Pagination<TDto>(param.pageNumber, param.pageSize, count, totalPages, items);
      }

      public async Task<bool> IsAnyByNameAndLocationGuidAsync(string name, Guid locationGuid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AnyAsync(x => (x.location == null || x.location.guid == locationGuid) && x.name == name, ct);
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AnyAsync(x => x.guid == guid, ct);
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AnyAsync(x => x.guid == guid && x.is_default, ct);
      }

      public async Task<bool> IsDisabledAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AnyAsync(x => x.guid == guid && x.is_active == false, ct);
      }

      public async Task<bool> IsEnabledAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                   .AnyAsync(x => x.guid == guid && x.is_active == true, ct);
      }

      public virtual async Task UpdateAsync(TDomain domain, CancellationToken ct = default)
      {
            var en = await Context.Set<TEntity>()
                  .Where(x => x.guid == domain.Guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(typeof(TEntity).Name, domain.Guid.ToString());

            updateEntity(en, domain);

            Context.Set<TEntity>().Update(en);
            await Context.SaveChangesAsync(ct);
      }

      // Helper method to build dynamic x => EF.Functions.ILike(x.Prop, "%term%") OR x.Prop.Contains("term")
      private static Expression<Func<TEntity, bool>> BuildSearchExpression(
          Expression<Func<TEntity, string>>[] searchFields,
          string searchTerm,
          bool isNpgsql)
      {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression? combinedBody = null;

            var pattern = $"%{searchTerm}%";
            var patternConstant = Expression.Constant(pattern);
            var searchConstant = Expression.Constant(searchTerm);

            foreach (var field in searchFields)
            {
                  // Replace field parameter with current root parameter
                  var memberAccess = ParameterReplacer.Replace(field.Body, field.Parameters[0], parameter);
                  Expression searchCall;

                  if (isNpgsql)
                  {
                        // EF.Functions.ILike(field, "%term%")
                        var functionsProp = Expression.Property(null, typeof(EF), nameof(EF.Functions));
                        var iLikeMethod = typeof(NpgsqlDbFunctionsExtensions).GetMethod(
                            nameof(NpgsqlDbFunctionsExtensions.ILike),
                            [typeof(DbFunctions), typeof(string), typeof(string)]
                        )!;
                        searchCall = Expression.Call(null, iLikeMethod, functionsProp, memberAccess, patternConstant);
                  }
                  else
                  {
                        // field.Contains("term")
                        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
                        searchCall = Expression.Call(memberAccess, containsMethod, searchConstant);
                  }

                  combinedBody = combinedBody is null ? searchCall : Expression.OrElse(combinedBody, searchCall);
            }

            return Expression.Lambda<Func<TEntity, bool>>(combinedBody!, parameter);
      }

      public async Task<short> GetFreeExeternalIdAsync(int max, IEnumerable<short>? exception = null, CancellationToken ct = default)
      {
            var existingIds = await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Select(x => x.external_id!.Value)
                  .ToArrayAsync(ct);

            var existIdsHash = existingIds.ToHashSet();

            if (exception != null)
            {
                  existIdsHash.UnionWith(exception);
            }

            for (short id = 1; id < max; id++)
            {
                  if (!existIdsHash.Contains(id))
                        return id;
            }

            throw new ExceedException(typeof(TEntity).Name);
      }

      public async Task<CommandDetailDto> GetCommandDetailAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(toCommand)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(typeof(TEntity).Name, guid.ToString());
      }

      public async Task<bool> IsAnyByNameAsync(string name, CancellationToken ct = default)
      {
             return await Context.Set<TEntity>()
                  .AsNoTracking()
                  .AnyAsync(x => x.name == name,ct);
      }

      public async Task<short> GetExternalIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.external_id)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(typeof(TEntity).Name, guid.ToString());
      }

      public async Task<IEnumerable<short>> GetRangeExernalIdByGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .Select(x => x.external_id!.Value)
                  .ToArrayAsync();
      }

      public virtual async Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(Guid guid, CancellationToken ct = default)
      {
           return await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Where(x => x.location == null || x.location.guid == guid)
                  .Select(x => new OptionDto(
                        x.name,
                        x.guid,
                        string.Empty
                  )).ToArrayAsync();
      }

      public async Task<IEnumerable<CommandDetailDto>> GetCommandDetailByLocationGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Where(x => x.location == null || x.location.guid == guid)
                  .Select(toCommand)
                  .ToArrayAsync(ct);
      }

      public async Task<Dictionary<Guid,(int id,short externalId)>> GetIdAndExternalIdMapGuidByGuidRangeAsync(IEnumerable<Guid> guids,CancellationToken ct = default)
      {
            var res = await Context.Set<TEntity>()
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .Select(x => new
                  {
                        guid=x.guid,
                        externalId=x.external_id,
                        id=x.id
                  })
                  .ToArrayAsync(cancellationToken: ct); 

            return res.ToDictionary(x => x.guid,x => (id:x.id,externalId:x.externalId ?? -1));
      }
}

// Utility class to rebind lambda expressions to a single root parameter
internal class ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam) : ExpressionVisitor
{
      protected override Expression VisitParameter(ParameterExpression node)
          => node == oldParam ? newParam : base.VisitParameter(node);

      public static Expression Replace(Expression body, ParameterExpression oldParam, ParameterExpression newParam)
          => new ParameterReplacer(oldParam, newParam).Visit(body);
}