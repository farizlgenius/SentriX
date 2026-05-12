using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Events;

public sealed record AssignPortEvent(string Mac, int Port) : IEvent;