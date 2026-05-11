using System;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record IsAnyWithMacQuery(string MacAddress) : IQuery<bool>;
