using SharedKernel.Messaging;

namespace Device.Contract.Command;
public sealed record DeleteRelayUsedCommand(short RelayNumber,int ModuleId) : ICommand;