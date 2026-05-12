using System;

namespace Operator.Contract.DTOs;

public record PasswordRuleDto(int Id,int Len,bool IsLower,bool IsUpper,bool IsSymbol,bool IsDigit,List<string> Weaks);