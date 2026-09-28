
using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Core.Contract.Commands.Device;

public sealed record ConfigurationStatusCommand(string mac,bool isSync,bool isUploaded) : ICommand;