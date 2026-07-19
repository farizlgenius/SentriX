using System;
using SharedKernel.Messaging;

namespace Adapter.Abstraction.Command;

public sealed record ModuleStatusCommand(Guid moduleGuid) : ICommand;
