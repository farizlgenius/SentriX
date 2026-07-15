using SharedKernel.Messaging;

namespace Device.Contract.Command;
public sealed record DeleteRelayUsedCommand(Guid Guid) : ICommand;