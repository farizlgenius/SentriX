using System;
using SharedKernel.Messaging;

namespace Device.Contract.Events;

public sealed record ComponentIdUpdatedEvent(int ComponentId, string Mac) : IEvent;
