using System;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record DeviceMacAndLocationIdByComponentIdQuery(int ComponentId) : IQuery<(string Mac, int LocationId)>;
