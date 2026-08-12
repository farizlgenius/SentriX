using Setting.Contract.DTOs.PasswordRule;
using Setting.Domain.Entities;

namespace Setting.Application.Interfaces;

public interface IPasswordRuleRepository : IBaseRepository
{
  Task<PasswordRuleDto> GetAsync(CancellationToken ct = default);
  Task UpdateAsync(PasswordRule entity, CancellationToken ct = default);
  Task<bool> ValidatePasswordWithRuleAsync(string password, CancellationToken ct = default);
}