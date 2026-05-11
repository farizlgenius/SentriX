using System;
using SharedKernel.Messaging;

namespace Operator.Contract.Queries;

public sealed record LocationListByUsernameQuery(string Username) : IQuery<List<int>>;
