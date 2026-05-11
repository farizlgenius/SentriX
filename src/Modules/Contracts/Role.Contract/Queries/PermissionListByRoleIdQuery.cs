using System;
using Role.Contract.DTOs;
using SharedKernel.Messaging;

namespace Role.Contract.Queries;

public sealed record PermissionListByRoleIdQuery(int RoleId) : IQuery<List<PermissionDto>>;
