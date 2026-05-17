using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;

namespace Adapter.Aero.Writer;

public sealed class CpWriter(ILogger<CpWriter> logger,IWriterRepository writer) : BaseWriter,ICpWriter
{
      public async Task<bool> OutputPointSpecification(OutputPointSpecification config)
      {
            CC_OP c = new CC_OP();
            c.lastModified = 0;
            c.scp_number = (short)config.aero.scp_id;
            c.sio_number = (short)config.sio_number;
            c.output = config.output;
            c.mode = config.mode;
            var result = Send((short)enCfgCmnd.enCcOutput,c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.OutputPointSpecification,(short)config.aero.scp_id));
                  await writer.AddWriterAuditAsync((short)config.aero.scp_id,config.aero.mac,WriterType.OutputPointSpecification,SCPDLL.scpGetTagLastPosted((short)config.aero.scp_id),MessageHelper.ToString(c));
                  return true;
                  
            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.OutputPointSpecification,(short)config.aero.scp_id));
                  await writer.AddWriterAuditAsync((short)config.aero.scp_id,config.aero.mac,WriterType.OutputPointSpecification,0,MessageHelper.ToString(c),WriterStatus.FAILED.ToString());
                  return false;
                 
            }
      }

      public async Task<bool> ControlPointConfiguration(ControlPointConfiguration config)
      {
            CC_CP c = new CC_CP();
            c.lastModified = 0;
            c.scp_number = (short)config.aero.scp_id;
            c.cp_number = config.cp_number;
            c.sio_number = config.sio_number;
            c.op_number = config.op_number;
            c.dflt_pulse = config.dflt_pulse;
             var result = Send((short)enCfgCmnd.enCcCP,c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.ControlPointConfiguration,(short)config.aero.scp_id));
                  await writer.AddWriterAuditAsync((short)config.aero.scp_id,config.aero.mac,WriterType.ControlPointConfiguration,SCPDLL.scpGetTagLastPosted((short)config.aero.scp_id),MessageHelper.ToString(c));
                  return true;
                  
            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.ControlPointConfiguration,(short)config.aero.scp_id));
                  await writer.AddWriterAuditAsync((short)config.aero.scp_id,config.aero.mac,WriterType.ControlPointConfiguration,0,MessageHelper.ToString(c),WriterStatus.FAILED.ToString());
                  return false;
                 
            }
      }
}