using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Command;

public sealed record ModuleStatusCommand(int DeviceCompnentId,string Mac,int ModuleComponentId) : ICommand;
