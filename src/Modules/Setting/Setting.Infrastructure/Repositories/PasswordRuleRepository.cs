using Microsoft.EntityFrameworkCore;
using Setting.Application.Interfaces;
using Setting.Contract.DTOs.PasswordRule;
using Setting.Domain.Entities;
using Setting.Infrastructure.Persistences;
using SharedKernel.Constants;
using SharedKernel.Exceptions;

namespace Setting.Infrastructure.Repositories;

public sealed class PasswordRuleRepository(SettingDbContext context) : IPasswordRuleRepository
{
  public async Task<PasswordRuleDto> GetAsync(CancellationToken ct = default)
  {
    return await context.PasswordRules
      .AsNoTracking()
      .Select(x => new PasswordRuleDto(
        x.guid,
        x.len,
        x.is_digit,
        x.is_lower,
        x.is_symbol,
        x.is_upper,
        x.weaks.Select(w => w.pattern).ToList()
      )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.PasswordRule, "null");
  }

  public async Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.PasswordRules
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid);
  }

  public async Task UpdateAsync(PasswordRule entity, CancellationToken ct = default)
  {
    var en = await context.PasswordRules
      .Include(x => x.weaks)
      .Where(x => x.guid == entity.Guid)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.PasswordRule, entity.Guid.ToString());

    en.len = entity.Len;
    en.is_digit = entity.IsDigit;
    en.is_lower = entity.IsLower;
    en.is_symbol = entity.IsSymbol;
    en.is_upper = entity.IsUpper;

    var incomingWeak = entity.WeakPassword
    .Select(x => x)
    .ToHashSet();

    foreach (var incoming in entity.WeakPassword)
    {

      var existing = en.weaks
          .Any(x => x.pattern.Equals(incoming));

      if (!existing)
      {
        en.weaks.Add(
          new Persistences.Entities.WeakPassword(
            new Domain.Entities.WeakPassword(incoming, en.guid)
          )
        );
      }
    }

    foreach (var existing in en.weaks.ToList())
    {

      if (!incomingWeak.Contains(existing.pattern))
      {
        var remove = await context.Set<Persistences.Entities.WeakPassword>()
        .Where(x => x.pattern.Equals(existing.pattern)).FirstOrDefaultAsync();

        context.Set<Persistences.Entities.WeakPassword>().Remove(remove ?? throw new NotFoundException(EntityType.WeakPassword, existing.pattern));

      }

    }

    context.PasswordRules.Update(en);

    await context.SaveChangesAsync(ct);

  }

  public async Task<bool> ValidatePasswordWithRuleAsync(string password, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}