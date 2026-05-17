using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Events;

public sealed record ModuleStatusByModuleIdEvent(int ModuleId) : IEvent;
