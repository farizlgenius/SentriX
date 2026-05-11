
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Persistences.Entities;
using Adapter.Aero.Writer;
using AeroAdapter.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Messaging;

namespace AeroAdapter.Infrastructure.Writer;

public sealed class MpWriter(ILogger<MpWriter> logger,IWriterRepository writer) : BaseWriter,IMpWriter
{
      
      public async Task<bool> InputPointSpecification(short ScpId,string Mac, InputPointSpecification spec)
      {
            CC_IP c = new CC_IP();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.sio_number = spec.sio_number;
            c.input = spec.input_number;
            c.icvt_num = spec.icvt_num;
            c.debounce = spec.debounce;
            c.hold_time = spec.hold_time;
            var result = Send((short)enCfgCmnd.enCcInput,c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.InputPointSpecification,ScpId));
                  await writer.AddWriterAuditAsync(ScpId,Mac,WriterType.InputPointSpecification,SCPDLL.scpGetTagLastPosted(ScpId),MessageHelper.ToString(c));
                  return true;
                  
            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.InputPointSpecification,ScpId));
                  await writer.AddWriterAuditAsync(ScpId,Mac,WriterType.InputPointSpecification,0,MessageHelper.ToString(c),WriterStatus.FAILED.ToString());
                  return false;
                 
            }
      }
}
