using System;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record LocationIdByMacQuery(string MacAddress) : IQuery<int>;