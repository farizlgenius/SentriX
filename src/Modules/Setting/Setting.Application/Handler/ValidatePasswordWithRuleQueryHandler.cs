using Setting.Application.Interfaces;
using Setting.Contract.DTOs.PasswordRule;
using Setting.Contract.Queries;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Setting.Application.Handler;

public sealed class ValidatePasswordWithRuleQueryHandler(IPasswordRuleRepository repo) : IQueryHandler<ValidatePasswordWithRuleQuery, string>
{
  public async Task<string> HandleAsync(ValidatePasswordWithRuleQuery query, CancellationToken ct)
  {
    var rule = await repo.GetAsync();

    return ValidationHelper.Password(
      query.password,
      rule.Len,
      rule.IsDigit,
      rule.IsSymbol,
      rule.IsUpper,
      rule.IsLower
    );
  }
}