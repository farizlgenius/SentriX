using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Events;

public sealed record AssignIpEvent(int ComponentId, string IpAddress) : IEvent;
