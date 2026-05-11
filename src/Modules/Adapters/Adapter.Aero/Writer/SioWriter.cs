
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Persistences.Entities;
using Adapter.Aero.Writer;
using AeroAdapter.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;

namespace AeroAdapter.Infrastructure.Writer;

public sealed class SioWriter(ILogger<SioWriter> logger, IWriterRepository writer) : BaseWriter,ISioWriter
{
      public async Task<bool> SioPanelConfiguration(short ScpId, string Mac, SioPanelConfiguration config)
      {
            CC_SIO c = new CC_SIO();
        c.lastModified = 0;
        c.scp_number = ScpId;
        c.sio_number = config.sio_number;
        c.nInputs = config.n_inputs;
        c.nOutputs = config.n_outputs;
        c.nReaders = config.n_readers;
        c.model = config.model;
        c.revision = 0;
        c.ser_num_low = 0;
        c.ser_num_high = -1;
        c.enable = config.enable;
        c.port = config.port;
        c.channel_out = 0;
        c.channel_in = 0;
        c.address = config.address;
        c.e_max = config.emax;
        c.flags = config.flags;
        c.nSioNextIn = config.n_sio_next_in;
        c.nSioNextOut = config.n_sio_next_out;
        c.nSioNextRdr = config.n_sio_next_rdr;
        c.nSioConnectTest = 0;
        c.nSioOemCode = 0;
        c.nSioOemMask = 0;
        var result = Send((short)enCfgCmnd.enCcSio,c);
        if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.SioPanelConfiguration,ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.SioPanelConfiguration, SCPDLL.scpGetTagLastPosted(ScpId), MessageHelper.ToJsonString(c));
                  return true;
                  
            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.SioPanelConfiguration,ScpId));
                  return false;
                 
            }
      }
}
