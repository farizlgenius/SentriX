using System;
using Device.Contract.DTOs;
using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record ModuleCreateCommand(ModuleDto command) : ICommand;