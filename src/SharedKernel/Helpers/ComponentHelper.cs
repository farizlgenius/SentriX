using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Helpers;

public static class ComponentHelper
{
      public static async Task<int> LowestUnassignedNumberAsync<TEntity>(
          DbContext context,
          Expression<Func<TEntity, bool>> filter,                 // WHERE condition (ex: mac == "xx")
          Expression<Func<TEntity, int>> numberSelector,          // column that stores numbers
          int max,
          CancellationToken ct = default)
          where TEntity : class
      {
            if (max <= 0) return -1;

            var query = context.Set<TEntity>()
                .AsNoTracking()
                .Where(filter)
                .Select(numberSelector);

            // If table is empty → start from 1
            if (!await query.AnyAsync(ct))
                  return 1;

            // Load only the number column
            var numbers = await query
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync(ct);

            int expected = 1;

            foreach (var num in numbers)
            {
                  if (num != expected)
                        return expected;

                  expected++;
            }

            return expected > max ? -1 : expected;
      }

      public static async Task<int> LowestUnassignedNumberAsync<TEntity>(
          DbContext context,
          Expression<Func<TEntity, int>> numberSelector,          // column that stores numbers
          int max,
          CancellationToken ct = default)
          where TEntity : class
      {
            if (max <= 0) return -1;

            var query = context.Set<TEntity>()
                .AsNoTracking()
                .Select(numberSelector);

            // If table is empty → start from 1
            if (!await query.AnyAsync(ct))
                  return 0;

            // Load only the number column
            var numbers = await query
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync(ct);

            int expected = 1;

            foreach (var num in numbers)
            {
                  if (num != expected)
                        return expected;

                  expected++;
            }

            return expected > max ? -1 : expected;
      }

      public static async Task<int> LowestUnassignedNumberAsync<TEntity>(
          DbContext context,
          Expression<Func<TEntity, bool>> filter,                 // WHERE condition (ex: mac == "xx")
          Expression<Func<TEntity, int>> numberSelector,          // column that stores numbers
          CancellationToken ct = default)
          where TEntity : class
      {

            var query = context.Set<TEntity>()
                .AsNoTracking()
                .Where(filter)
                .Select(numberSelector);

            // If table is empty → start from 1
            if (!await query.AnyAsync(ct))
                  return 1;

            // Load only the number column
            var numbers = await query
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync(ct);

            int expected = 1;

            foreach (var num in numbers)
            {
                  if (num != expected)
                        return expected;

                  expected++;
            }

            return expected;
      }

      public static async Task<int> LowestUnassignedNumberAsync<TEntity>(
          DbContext context,
          List<int> Except,
          Expression<Func<TEntity, bool>> filter,                 // WHERE condition (ex: mac == "xx")
          Expression<Func<TEntity, int>> numberSelector,
          int max,          // column that stores numbers
          CancellationToken ct = default)
          where TEntity : class
      {

            if (max <= 0) return -1;

            var query = context.Set<TEntity>()
                .AsNoTracking()
                .Where(filter)
                .Select(numberSelector);

            // If table is empty → start from 1
            if (!await query.AnyAsync(ct))
                  return 0;

            // Load only the number column
            var numbers = await query
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync(ct);

            int expected = 1;

            foreach (var num in numbers)
            {
                  if (num != expected && !Except.Contains(num))
                        return expected;

                  expected++;
            }

            return expected > max ? -1 : expected;
      }

      public static async Task<int> LowestUnassignedNumberAsync<TEntity>(
    DbContext context,
    List<int> Except,
    Expression<Func<TEntity, bool>> filter,
    Expression<Func<TEntity, object>> numberSelector,
    int max,
    CancellationToken ct = default)
    where TEntity : class
      {
            if (max <= 0)
                  return -1;

            var rows = await context.Set<TEntity>()
                .AsNoTracking()
                .Where(filter)
                .Select(numberSelector)
                .ToListAsync(ct);

            var numbers = rows
                .SelectMany(x =>
                {
                      return x.GetType()
                  .GetProperties()
                  .Select(p => (int)p.GetValue(x)!);
                })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            if (numbers.Count == 0)
                  return 0;

            int expected = 1;

            foreach (var num in numbers)
            {
                  if (num != expected && !Except.Contains(expected))
                        return expected;

                  expected++;
            }

            return expected > max ? -1 : expected;
      }
}