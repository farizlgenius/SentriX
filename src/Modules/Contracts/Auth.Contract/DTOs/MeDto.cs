using System;
using System.Net;
using Core.Contract.DTOs.Role;
using SharedKernel.Domain;

namespace Auth.Contract.DTOs;

public sealed record MeDto(Guid Guid, string Username, IEnumerable<Guid> LocationGuids, IEnumerable<ModulePermissionDto> Permissions);