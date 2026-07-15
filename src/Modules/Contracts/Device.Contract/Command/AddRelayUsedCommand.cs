using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record AddRelayUsedCommand(short RelayNumber,Guid ModuleGuid,int LocationId) : ICommand;