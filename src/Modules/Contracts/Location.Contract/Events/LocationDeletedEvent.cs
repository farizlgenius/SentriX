using System;
using SharedKernel.Messaging;

namespace Location.Contract.Events;

public sealed record LocationDeletedEvent(int LocationId) : IEvent;
