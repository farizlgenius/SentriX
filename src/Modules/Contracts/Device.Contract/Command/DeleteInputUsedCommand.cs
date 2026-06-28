using SharedKernel.Messaging;

namespace Device.Contract.Command;
public sealed record DeleteInputUsedCommand(short InputNumber,int ModuleId) : ICommand;