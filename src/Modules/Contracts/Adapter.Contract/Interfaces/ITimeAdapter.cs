using System;
using System.Text.Json;
using Core.Contract.DTOs.Time;
using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface ITimeAdapter
{

      Task TimeZone(
            string mac,
            string ip,
            short externalId,
            short tzExternalId,
            List<IntervalDto> intervals,
            CancellationToken ct = default
            );

}
