using System.Text.Json;
using System.Xml;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Output.Contract.DTOs;
using SharedKernel.Domain;

namespace Adapter.Aero.Services;

public sealed class AeroControlService(ILogger<AeroControlService> logger,IOutputCommand writer) : IControlAdapter
{
      public async Task CreateAsync(CreateOutputDto dto)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<OptionDto>> GetRelayModeAsync()
      {
            throw new NotImplementedException();
      }

      public Task TriggerOutputAsync(int moduleId, int Command)
      {
            throw new NotImplementedException();
      }
}