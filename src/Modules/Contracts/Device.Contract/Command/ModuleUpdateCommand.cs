using System;
using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record ModuleUpdateCommand(string Mac,int Id,string SerialNumber,string Fw,int Port) : ICommand;
