using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Events;

public sealed record ModuleStatusByDeviceIdEvent(string Mac,int ComponentIds) : IEvent;
