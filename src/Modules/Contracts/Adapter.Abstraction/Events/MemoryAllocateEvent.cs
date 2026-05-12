using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Events;

public sealed record MemoryAllocateEvent(string Mac,string Status) : IEvent;
