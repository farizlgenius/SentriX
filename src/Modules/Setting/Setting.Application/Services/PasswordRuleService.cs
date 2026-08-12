using System.Dynamic;
using Setting.Application.Interfaces;
using Setting.Contract.DTOs.PasswordRule;
using Setting.Contract.Interfaces;
using SharedKernel.Constants;
using SharedKernel.Exceptions;

namespace Setting.Application.Services;

public sealed class PasswordRuleService(IPasswordRuleRepository repo) : IPasswordRule
{
  public async Task<PasswordRuleDto> GetAsync(CancellationToken ct = default)
  {
    return await repo.GetAsync(ct);
  }

  public async Task<PasswordRuleDto> UpdateAsync(UpdatePasswordRuleDto dto, CancellationToken ct = default)
  {

    var d = new Domain.Entities.PasswordRule(
      dto.Guid,
      dto.Len,
      dto.IsDigit,
      dto.IsLower,
      dto.IsSymbol,
      dto.IsUpper,
      dto.Weaks
    );

    if (!await repo.IsAnyByGuidAsync(dto.Guid))
      throw new NotFoundException(EntityType.PasswordRule, dto.Guid.ToString());

    await repo.UpdateAsync(d, ct);

    return new PasswordRuleDto(
      d.Guid,
      d.Len,
      d.IsDigit,
      d.IsLower,
      d.IsSymbol,
      d.IsUpper,
      d.WeakPassword
    );
  }
}