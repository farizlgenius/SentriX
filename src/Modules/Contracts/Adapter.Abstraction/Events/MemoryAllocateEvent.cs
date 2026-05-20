using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Events;

public sealed record MemoryAllocateEvent(int ComponentId,string Status) : IEvent;
