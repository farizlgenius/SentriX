using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record AddRelayUsedCommand(short RelayNumber,int ModuleId,int LocationId) : ICommand<bool>;