using System;
using Device.Contract.DTOs;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record IdByComponentIdQuery(int ComponentId) : IQuery<int>;