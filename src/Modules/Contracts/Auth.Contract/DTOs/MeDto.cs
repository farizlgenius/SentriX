using System;
using System.Net;
using Role.Contract.DTOs;
using SharedKernel.Domain;

namespace Auth.Contract.DTOs;

public sealed record MeDto(List<int> Locations, List<PermissionDto> Permissions);