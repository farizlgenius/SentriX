using System;
using System.Net;
using Core.Contract.DTOs.Role;
using SharedKernel.Domain;

namespace Auth.Contract.DTOs;

public sealed record MeDto(IEnumerable<Guid> LocationGuids, IEnumerable<PermissionDto> Permissions);