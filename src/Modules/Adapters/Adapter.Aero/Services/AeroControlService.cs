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

namespace Adapter.Aero.Services;

public sealed class AeroControlService(ILogger<AeroControlService> logger,ISioRepository sioRepository,ICpRepository repo,ICpWriter writer) : IControlAdapter
{
      public async Task CreateAsync(CreateOutputDto dto)
      {
            var cpMetadata = JsonSerializer.Deserialize<CpMetadata>(dto.Metadata);
            if(cpMetadata == null)
            {
                  logger.LogError("Deserialize metadata failed.");
                  throw new Exception("Deserialize metadata failed.");
            }
            var aero_id = await sioRepository.GetAeroIdByModuleIdAsync(dto.ModuleId);
            var sio = await sioRepository.GetSioPanelConfigurationByModuleIdAsync(dto.ModuleId);
            // Create Outout Spec
            var oSpec = new OutputPointSpecification(
                  aero_id,
                  sio.sio_number,
                  cpMetadata.OutputNo,
                  UtilitiesHelper.OutputModeResolver(
                        (RelayMode)cpMetadata.RelayMode,
                        (RelayOfflineMode)cpMetadata.OfflineMode
                        )
            );
            var oRes = await repo.AddOutputPointSpeicificationAsync(oSpec);

            // Send Command here
            await writer.OutputPointSpecification(oRes);
            


            // Create Control point
            var cSpec = new ControlPointConfiguration(
                  aero_id,
                  0,
                  sio.sio_number,
                  cpMetadata.OutputNo,
                  (short)cpMetadata.DefaultPulse
            );
            var cRes = await repo.AddControlPointConfigurationAsync(cSpec);

            // Send Comand here
            await writer.ControlPointConfiguration(cRes);

      }
}