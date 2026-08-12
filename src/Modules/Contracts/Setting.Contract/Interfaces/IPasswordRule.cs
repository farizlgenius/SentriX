using Setting.Contract.DTOs.PasswordRule;

namespace Setting.Contract.Interfaces;

public interface IPasswordRule
{
  Task<PasswordRuleDto> GetAsync(CancellationToken ct = default);
  Task<PasswordRuleDto> UpdateAsync(UpdatePasswordRuleDto dto, CancellationToken ct = default);
}