using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Query;

public sealed record MaxModuleByMacQuery(string Mac) : IQuery<int>;
