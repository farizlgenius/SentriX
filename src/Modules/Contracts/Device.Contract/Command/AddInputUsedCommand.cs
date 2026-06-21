using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record AddInputUsedCommand(short InputNumber,int ModuleId,int LocationId) : ICommand<bool>;