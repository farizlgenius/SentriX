using System;

namespace Company.Contract.DTOs;

public record CompanyDto(int Id, string Name, string Description, string Address);