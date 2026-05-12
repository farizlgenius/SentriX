using System;
using SharedKernel.Messaging;

namespace Location.Contract.Queries;

public sealed record LocationNameByIdQuery(int LocationId) : IQuery<string>;
