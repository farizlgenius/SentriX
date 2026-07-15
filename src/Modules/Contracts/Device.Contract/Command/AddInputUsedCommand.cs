using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record AddInputUsedCommand(short InputNumber,Guid ModuleGuid,int LocationId) : ICommand;