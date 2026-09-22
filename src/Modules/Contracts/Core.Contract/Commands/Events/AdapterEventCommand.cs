
using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Core.Contract.Commands.Events;

public sealed record AdapterEventCommand(CommandResponse @event) : ICommand;