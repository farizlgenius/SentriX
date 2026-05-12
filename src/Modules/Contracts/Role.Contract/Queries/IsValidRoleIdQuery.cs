using System;
using SharedKernel.Messaging;

namespace Role.Contract.Queries;

public sealed record IsValidRoleIdQuery(int RoleId) : IQuery<bool>;
