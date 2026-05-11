using System;
using SharedKernel.Messaging;

namespace Location.Contract.Events;

public sealed record LocationCreatedEvent(int LocationId) : IEvent;
