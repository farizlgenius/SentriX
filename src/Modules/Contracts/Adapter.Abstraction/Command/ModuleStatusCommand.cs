using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Command;

public sealed record ModuleStatusCommand(long DeviceCompnentId,string Mac,int ModuleComponentId) : ICommand;
