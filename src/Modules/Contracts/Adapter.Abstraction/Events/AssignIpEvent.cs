using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Events;

public sealed record AssignIpEvent(string Mac, string IpAddress) : IEvent;
