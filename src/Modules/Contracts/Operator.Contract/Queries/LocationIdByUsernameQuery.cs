using System;
using SharedKernel.Messaging;

namespace Operator.Contract.Queries;

public sealed record LocationIdByUsernameQuery(string Username) : IQuery<int>;