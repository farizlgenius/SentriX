using System;
using SharedKernel.Messaging;

namespace Notifier.Contract.Events;

public sealed record ConfigEvent(object config) : IEvent;
