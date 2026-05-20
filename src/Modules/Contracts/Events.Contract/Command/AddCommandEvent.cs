using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Events.Contract.Command;

public sealed record AddCommandEvent(CommandResponse res) : ICommand;