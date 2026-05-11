using System;

namespace Company.Contract.DTOs;

public sealed record CreateCompanyDto(string Name, string Address, string Description);