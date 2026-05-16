using System;
using SharedKernel.Messaging;

namespace Notifier.Contract.Events;

public sealed record ExceptionEvent(string Message) : IEvent;
