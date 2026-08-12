using Setting.Contract.DTOs.PasswordRule;
using SharedKernel.Messaging;

namespace Setting.Contract.Queries;

public sealed record ValidatePasswordWithRuleQuery(string password) : IQuery<string>;