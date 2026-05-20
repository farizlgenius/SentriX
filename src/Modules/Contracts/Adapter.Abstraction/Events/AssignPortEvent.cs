using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Events;

public sealed record AssignPortEvent(int ComponentId, int Port) : IEvent;